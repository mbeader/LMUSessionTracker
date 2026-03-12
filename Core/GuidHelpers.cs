using System;

namespace LMUSessionTracker.Core {
	// Temporary UUIDv7 implementation based on dotnet's
	// Licensed to the .NET Foundation under one or more agreements.
	// The .NET Foundation licenses this file to you under the MIT license.
	// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Guid.cs
	// commit e528a6e
	public static class GuidHelpers {
		private const byte Variant10xxMask = 0xC0;
		private const byte Variant10xxValue = 0x80;

		private const ushort VersionMask = 0xF000;
		private const ushort Version7Value = 0x7000;

		public static Guid CreateVersion7() => CreateVersion7(DateTimeOffset.UtcNow);

		public static Guid CreateVersion7(DateTimeOffset timestamp) {
			Guid result = Guid.NewGuid();

			// 2^48 is roughly 8925.5 years, which from the Unix Epoch means we won't
			// overflow until around July of 10,895. So there isn't any need to handle
			// it given that DateTimeOffset.MaxValue is December 31, 9999. However, we
			// can't represent timestamps prior to the Unix Epoch since UUIDv7 explicitly
			// stores a 48-bit unsigned value, so we do need to throw if one is passed in.

			long unix_ts_ms = timestamp.ToUnixTimeMilliseconds();
			ArgumentOutOfRangeException.ThrowIfNegative(unix_ts_ms, nameof(timestamp));

			byte[] bytes = result.ToByteArray();

			int a = (int)(unix_ts_ms >> 16);
			short b = (short)(unix_ts_ms);
			short c = (short)(((((bytes[6] << 8) & 0xFF00) + bytes[7]) & ~VersionMask) | Version7Value);
			byte d = (byte)((bytes[8] & ~Variant10xxMask) | Variant10xxValue);

			return new Guid(a, b, c, d, bytes[9], bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15]);
		}
	}
}
