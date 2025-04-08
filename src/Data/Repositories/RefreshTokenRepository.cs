using Dapper;
using System.Data;

namespace Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbConnection _con;
        private readonly string _refreshTokensTable = "refresh_tokens";

        public RefreshTokenRepository(IDbConnection con)
        {
            _con = con;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken token)
        {
            var sql = $@"
INSERT INTO {_refreshTokensTable}
(
    token_id,
    chat_id,
    token_hash,
    created_at,
    expires_at
)
VALUES
(
    @Id,
    @UserId,
    @TokenHash,
    @CreatedAt,
    @ExpiresAt
)";
            await _con.ExecuteAsync(sql, token);
            return token;
        }

        public async Task<bool> IsOkAsync(string tokenHash, long userId)
        {
            var sql = $@"
SELECT COUNT(1)
FROM {_refreshTokensTable}
WHERE token_hash = @tokenHash
    AND expires_at > @now
    AND chat_id = @userId";

            var count = await _con.ExecuteScalarAsync<int>(sql, new { tokenHash, now = DateTime.UtcNow, userId });
            return count > 0;
        }

        public async Task<bool> RemoveAsync(string tokenHash)
        {
            var sql = $@"
DELETE FROM {_refreshTokensTable}
WHERE token_hash = @tokenHash
RETURNING chat_id";

            var removedChatId = await _con.ExecuteScalarAsync<string>(sql, new { tokenHash });
            return !string.IsNullOrEmpty(removedChatId);
        }

        public async Task<bool> RemoveIfOkAsync(string tokenHash, long userId)
        {
            var sql = $@"
DELETE FROM {_refreshTokensTable}
WHERE token_hash = @tokenHash
    AND expires_at > @now
    AND chat_id = @userId
RETURNING chat_id";

            var removedChatId = await _con.ExecuteScalarAsync<string>(sql, new { tokenHash, now = DateTime.UtcNow, userId });
            return !string.IsNullOrEmpty(removedChatId);
        }
    }
}

