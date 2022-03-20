using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Infrastructure.Storages;
using AuthenticationService.Infrastructure.Configurations.Repository;
using ServiceStack.Redis;

namespace AuthenticationService.Infrastructure.Storages.TokenBlacklist.Redis
{

    public class RedisTokenBlacklistStorage : ITokenBlacklistStorage
    {
        // Should there be 2 separated blacklists one for Access tokens and another for Refresh tokens? 
        private readonly RedisTokenBlacklistConfiguration _redisTokenBlacklistConfiguration;
        private readonly RedisManagerPool _redisManagerPool;

        public RedisTokenBlacklistStorage(
            RedisTokenBlacklistConfiguration redisTokenBlacklistConfiguration
        )
        {
            _redisTokenBlacklistConfiguration = redisTokenBlacklistConfiguration ??
                throw new ArgumentNullException(nameof(redisTokenBlacklistConfiguration));
            
            _redisManagerPool = new RedisManagerPool(redisTokenBlacklistConfiguration.ConnectionString);
            
        }

        public async Task AddTokensAsync(Dictionary<string, int> tokenToExpirationTimeMapping)
        {
            await removeExpiredTokensAsync();  // Cleaning expired tokens up when new tokens are added
            
            var redisClient = await _redisManagerPool.GetClientAsync();

            foreach (KeyValuePair<string, int> tokenToExpiration in tokenToExpirationTimeMapping) {
                await redisClient.AddItemToSortedSetAsync(
                    setId: _redisTokenBlacklistConfiguration.TokenBlacklistSortedSetName,
                    value: tokenToExpiration.Key,
                    score: tokenToExpiration.Value
                ); 
            }  
        }

        public async Task<bool> CheckTokenInBlacklistAsync(Dictionary<string, int> tokenToExpirationTimeMapping)
        {
            List<string> tokens;

            var redisClient =  await _redisManagerPool.GetClientAsync();

            foreach(KeyValuePair<string, int> tokenToExpiration in tokenToExpirationTimeMapping) {
                    tokens = await redisClient.GetRangeFromSortedSetByHighestScoreAsync(
                    setId: _redisTokenBlacklistConfiguration.TokenBlacklistSortedSetName,
                    fromScore: tokenToExpiration.Value,
                    toScore: tokenToExpiration.Value
                );
                    
                    if (tokens.Exists(token => token == tokenToExpiration.Key)) {
                       return true;
                    }
                }
            return false;
        }

        public void Dispose()
        {
            _redisManagerPool.Dispose();
        }

        private async Task removeExpiredTokensAsync()
        {
            var redisClient = await _redisManagerPool.GetClientAsync();

            await redisClient.RemoveRangeFromSortedSetByScoreAsync(
                setId: _redisTokenBlacklistConfiguration.TokenBlacklistSortedSetName,
                fromScore:  0,
                toScore: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            );
        }
    }
}