using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Files.Helpers.Shared;

public static class ChecksumHelpers
{
	public static string CalculateChecksumForPath(string path)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(path);
		Span<byte> span = stackalloc byte[16];
		MD5.HashData(bytes, span);
		return Convert.ToHexString(span);
	}

	public static async Task<string> CreateCRC32(Stream stream, CancellationToken cancellationToken)
	{
		Crc32 crc32 = new Crc32();
		await crc32.AppendAsync(stream, cancellationToken);
		byte[] currentHash = crc32.GetCurrentHash();
		Array.Reverse(currentHash);
		return Convert.ToHexString(currentHash);
	}

	public static async Task<string> CreateMD5(Stream stream, CancellationToken cancellationToken)
	{
		return Convert.ToHexString(await MD5.HashDataAsync(stream, cancellationToken)).ToLower();
	}

	public static async Task<string> CreateSHA1(Stream stream, CancellationToken cancellationToken)
	{
		return Convert.ToHexString(await SHA1.HashDataAsync(stream, cancellationToken)).ToLower();
	}

	public static async Task<string> CreateSHA256(Stream stream, CancellationToken cancellationToken)
	{
		return Convert.ToHexString(await SHA256.HashDataAsync(stream, cancellationToken)).ToLower();
	}

	public static async Task<string> CreateSHA384(Stream stream, CancellationToken cancellationToken)
	{
		return Convert.ToHexString(await SHA384.HashDataAsync(stream, cancellationToken)).ToLower();
	}

	public static async Task<string> CreateSHA512(Stream stream, CancellationToken cancellationToken)
	{
		return Convert.ToHexString(await SHA512.HashDataAsync(stream, cancellationToken)).ToLower();
	}

	public static async Task<Dictionary<string, string>> CreateHashesForPathAsync(string path, CancellationToken cancellationToken)
	{
		Dictionary<string, string> result;
		await using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1048576, FileOptions.Asynchronous | FileOptions.SequentialScan))
		{
			Crc32 crc32 = new Crc32();
			using IncrementalHash md5 = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
			using IncrementalHash sha1 = IncrementalHash.CreateHash(HashAlgorithmName.SHA1);
			using IncrementalHash sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
			using IncrementalHash sha384 = IncrementalHash.CreateHash(HashAlgorithmName.SHA384);
			using IncrementalHash sha512 = IncrementalHash.CreateHash(HashAlgorithmName.SHA512);
			byte[] buffer = new byte[1048576];
			int length;
			while ((length = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken)) > 0)
			{
				Span<byte> span = buffer.AsSpan(0, length);
				crc32.Append(span);
				md5.AppendData(span);
				sha1.AppendData(span);
				sha256.AppendData(span);
				sha384.AppendData(span);
				sha512.AppendData(span);
			}
			byte[] currentHash = crc32.GetCurrentHash();
			Array.Reverse(currentHash);
			result = new Dictionary<string, string>
			{
				["CRC32"] = Convert.ToHexString(currentHash),
				["MD5"] = Convert.ToHexString(md5.GetHashAndReset()).ToLowerInvariant(),
				["SHA1"] = Convert.ToHexString(sha1.GetHashAndReset()).ToLowerInvariant(),
				["SHA256"] = Convert.ToHexString(sha256.GetHashAndReset()).ToLowerInvariant(),
				["SHA384"] = Convert.ToHexString(sha384.GetHashAndReset()).ToLowerInvariant(),
				["SHA512"] = Convert.ToHexString(sha512.GetHashAndReset()).ToLowerInvariant()
			};
		}
		return result;
	}
}
