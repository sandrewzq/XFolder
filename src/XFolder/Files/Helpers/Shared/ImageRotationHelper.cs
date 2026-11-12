using System;
using System.IO;
using SkiaSharp;

namespace Files.Helpers.Shared;

internal static class ImageRotationHelper
{
	public static bool CanRotateImage(string? fileExtension)
	{
		if (string.IsNullOrWhiteSpace(fileExtension))
		{
			return false;
		}
		if (!fileExtension.Equals(".png", StringComparison.OrdinalIgnoreCase) && !fileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) && !fileExtension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) && !fileExtension.Equals(".jfif", StringComparison.OrdinalIgnoreCase) && !fileExtension.Equals(".bmp", StringComparison.OrdinalIgnoreCase) && !fileExtension.Equals(".webp", StringComparison.OrdinalIgnoreCase))
		{
			return fileExtension.Equals(".gif", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public static bool Rotate90(string filePath, bool clockwise)
	{
		if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
		{
			return false;
		}
		if (!TryGetEncodeFormat(Path.GetExtension(filePath), out var format, out var quality))
		{
			return false;
		}
		string text = filePath + ".rotate.tmp";
		try
		{
			using SKBitmap sKBitmap = SKBitmap.Decode(filePath);
			if (sKBitmap == null)
			{
				return false;
			}
			using SKBitmap sKBitmap2 = new SKBitmap(new SKImageInfo(sKBitmap.Height, sKBitmap.Width, sKBitmap.ColorType, sKBitmap.AlphaType, sKBitmap.ColorSpace));
			using (SKCanvas sKCanvas = new SKCanvas(sKBitmap2))
			{
				if (clockwise)
				{
					sKCanvas.Translate(sKBitmap2.Width, 0f);
					sKCanvas.RotateDegrees(90f);
				}
				else
				{
					sKCanvas.Translate(0f, sKBitmap2.Height);
					sKCanvas.RotateDegrees(-90f);
				}
				sKCanvas.DrawBitmap(sKBitmap, 0f, 0f);
				sKCanvas.Flush();
			}
			using SKImage sKImage = SKImage.FromBitmap(sKBitmap2);
			using SKData sKData = sKImage.Encode(format, quality);
			if (sKData == null)
			{
				return false;
			}
			using (FileStream target = File.Open(text, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				sKData.SaveTo(target);
			}
			File.Move(text, filePath, overwrite: true);
			return true;
		}
		catch
		{
			return false;
		}
		finally
		{
			try
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			catch
			{
			}
		}
	}

	private static bool TryGetEncodeFormat(string? extension, out SKEncodedImageFormat format, out int quality)
	{
		quality = 92;
		string text = extension?.ToLowerInvariant();
		if (text != null)
		{
			int length = text.Length;
			if (length != 4)
			{
				if (length == 5)
				{
					char c = text[2];
					if (c != 'e')
					{
						if (c != 'f')
						{
							if (c == 'p' && text == ".jpeg")
							{
								goto IL_00e7;
							}
						}
						else if (text == ".jfif")
						{
							goto IL_00e7;
						}
					}
					else if (text == ".webp")
					{
						format = SKEncodedImageFormat.Webp;
						quality = 90;
						return true;
					}
				}
			}
			else
			{
				char c = text[1];
				if ((uint)c <= 103u)
				{
					switch (c)
					{
					case 'b':
						if (!(text == ".bmp"))
						{
							break;
						}
						format = SKEncodedImageFormat.Bmp;
						quality = 100;
						return true;
					case 'g':
						if (!(text == ".gif"))
						{
							break;
						}
						format = SKEncodedImageFormat.Gif;
						quality = 100;
						return true;
					}
				}
				else if (c != 'j')
				{
					if (c == 'p' && text == ".png")
					{
						format = SKEncodedImageFormat.Png;
						quality = 100;
						return true;
					}
				}
				else if (text == ".jpg")
				{
					goto IL_00e7;
				}
			}
		}
		format = SKEncodedImageFormat.Bmp;
		return false;
		IL_00e7:
		format = SKEncodedImageFormat.Jpeg;
		return true;
	}
}
