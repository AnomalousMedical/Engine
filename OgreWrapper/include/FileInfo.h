#pragma once

namespace Rendering
{

/// <summary>
/// Information about a file/directory within the archive will be returned using
/// a FileInfo class.
/// </summary>
public ref class FileInfo
{
private:
	System::String^ filename;
	System::String^ path;
	System::String^ baseName;
	size_t compressedSize;
	size_t uncompressedSize;

internal:
	/// <summary>
	/// 
	/// </summary>
	/// <param name="filename"></param>
	/// <param name="path"></param>
	/// <param name="baseName"></param>
	/// <param name="compressedSize"></param>
	/// <param name="uncompressedSize"></param>
	FileInfo(System::String^ filename, System::String^ path, System::String^ baseName, size_t compressedSize, size_t uncompressedSize)
		:filename(filename), path(path), baseName(baseName), compressedSize(compressedSize), uncompressedSize(uncompressedSize)
	{

	}

public:

	/// <summary>
	/// The file's fully qualified name.
	/// </summary>
	property System::String^ Filename 
	{
		System::String^ get()
		{
			return filename;
		}
	}

	/// <summary>
	/// Path name; separated by '/' and ending with '/'. 
	/// </summary>
	property System::String^ Path 
	{
		System::String^ get()
		{
			return path;
		}
	}

	/// <summary>
	/// Base filename. 
	/// </summary>
	property System::String^ Basename 
	{
		System::String^ get()
		{
			return baseName;
		}
	}

	/// <summary>
	/// Compressed size. 
	/// </summary>
	property size_t CompressedSize 
	{
		size_t get()
		{
			return compressedSize;
		}
	}

	/// <summary>
	/// Uncompressed size. 
	/// </summary>
	property size_t UncompressedSize 
	{
		size_t get()
		{
			return uncompressedSize;
		}
	}
};

}