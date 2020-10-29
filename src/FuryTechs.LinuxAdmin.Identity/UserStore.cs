using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FuryTechs.LinuxAdmin.Identity {
    public class UserStore : IUserStore<User> {
        public UserStore () { }

        public Task<string> GetUserIdAsync (User user, CancellationToken cancellationToken) {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync (User user, CancellationToken cancellationToken) {
            return Task.FromResult (user.UserName);
        }

        public Task SetUserNameAsync (User user, string userName, CancellationToken cancellationToken) {
            throw new System.NotSupportedException ();
        }

        public Task<string> GetNormalizedUserNameAsync (User user, CancellationToken cancellationToken) {
            return Task.FromResult (user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync (User user, string normalizedName,
            CancellationToken cancellationToken) {
            throw new System.NotSupportedException ();
        }

        public Task<IdentityResult> CreateAsync (User user, CancellationToken cancellationToken) {
            throw new System.NotSupportedException ();
        }

        public Task<IdentityResult> UpdateAsync (User user, CancellationToken cancellationToken) {
            throw new System.NotSupportedException ();
        }

        public Task<IdentityResult> DeleteAsync (User user, CancellationToken cancellationToken) {
            throw new System.NotSupportedException ();
        }

        public Task<User> FindByIdAsync (string userId, CancellationToken cancellationToken) {
            throw new System.NotImplementedException ();
        }

        public async Task<User> FindByNameAsync (string normalizedUserName, CancellationToken cancellationToken) {
            using var proc = new Process () {
                StartInfo = new ProcessStartInfo {
                FileName = "/bin/bash",
                Arguments = "-c \"getent passwd\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
                }
            };
            proc.Start ();

            try {
                while (!proc.StandardOutput.EndOfStream) {
                    var line = await proc.StandardOutput.ReadLineAsync ();
                    if (!string.IsNullOrWhiteSpace (line)) {
                        var segments = line.Split (':');
                        if (segments[0].ToUpperInvariant () == normalizedUserName) {
                            bool isLocked = segments[6] == "/usr/bin/nologin";
                            return new User {
                                UserName = segments[0],
                                    NormalizedUserName = segments[0].ToLowerInvariant (),
                                    Id = segments[2],
                                    LockoutEnabled = isLocked,
                                    LockoutEnd = isLocked ? DateTimeOffset.MaxValue : DateTimeOffset.MinValue
                            };
                        }
                    }
                }
            } finally {
                proc.WaitForExit ();
            }
            return null;
        }

        public void Dispose () { }
    }
}
