#pragma once

namespace Ogre
{
	class HardwareBuffer;
}

namespace OgreWrapper{

/// <summary>
/// A wrapper class for the Ogre::HardwareBuffer
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class HardwareBuffer
{
public:
/// <summary>
/// Enums describing hardware buffer usage; not mutually exclusive.
/// </summary>
[Engine::Attributes::SingleEnum]
enum class Usage : unsigned int
{
	/// <summary>
	/// Static buffer which the application rarely modifies once created.
    /// Modifying the contents of this buffer will involve a performance hit.
	/// </summary>
	HBU_STATIC = 1,

	/// <summary>
	/// Indicates the application would like to modify this buffer with the CPU
    /// fairly often. Buffers created with this flag will typically end up in
    /// AGP memory rather than video memory.
	/// </summary>
	HBU_DYNAMIC = 2,

	/// <summary>
	/// Indicates the application will never read the contents of the buffer back, 
    /// it will only ever write data. Locking a buffer with this flag will ALWAYS 
    /// return a pointer to new, blank memory rather than the memory associated 
    /// with the contents of the buffer; this avoids DMA stalls because you can 
    /// write to a new memory area while the previous one is being used. 
	/// </summary>
	HBU_WRITE_ONLY = 4,

	/// <summary>
	/// Indicates that the application will be refilling the contents
    /// of the buffer regularly (not just updating, but generating the
    /// contents from scratch), and therefore does not mind if the contents 
    /// of the buffer are lost somehow and need to be recreated. This
    /// allows and additional level of optimisation on the buffer.
    /// This option only really makes sense when combined with 
    /// HBU_DYNAMIC_WRITE_ONLY.
	/// </summary>
	HBU_DISCARDABLE = 8,

	/// <summary>
	/// Combination of HBU_STATIC and HBU_WRITE_ONLY
	/// </summary>
	HBU_STATIC_WRITE_ONLY = 5, 

	/// <summary>
	/// Combination of HBU_DYNAMIC and HBU_WRITE_ONLY. If you use 
    /// this, strongly consider using HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE
    /// instead if you update the entire contents of the buffer very 
    /// regularly. 
	/// </summary>
	HBU_DYNAMIC_WRITE_ONLY = 6,
    
	/// <summary>
	/// Combination of HBU_DYNAMIC, HBU_WRITE_ONLY and HBU_DISCARDABLE
	/// </summary>
	HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE = 14


};

/// <summary>
/// Hardware buffer locking options.
/// </summary>
[Engine::Attributes::SingleEnum]
enum class LockOptions : unsigned int
{
	/// <summary>
	/// Normal mode, ie allows read/write and contents are preserved. 
	/// </summary>
	HBL_NORMAL,

	/// <summary>
	/// Discards the <em>entire</em> buffer while locking; this allows optimisation to be 
	/// performed because synchronisation issues are relaxed. Only allowed on buffers 
    /// created with the HBU_DYNAMIC flag.
	/// </summary>
	HBL_DISCARD,

	/// <summary>
	/// Lock the buffer for reading only. Not allowed in buffers which are created with HBU_WRITE_ONLY. 
	/// Mandatory on static buffers, i.e. those created without the HBU_DYNAMIC flag. 
	/// </summary>
	HBL_READ_ONLY,

	/// <summary>
	/// As HBL_NORMAL, except the application guarantees not to overwrite any 
    /// region of the buffer which has already been used in this frame, can allow
    /// some optimisation on some APIs.
	/// </summary>
    HBL_NO_OVERWRITE
};

private:
	Ogre::HardwareBuffer* hardwareBuffer;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="hardwareBuffer">The hardware buffer to wrap.</param>
	HardwareBuffer(Ogre::HardwareBuffer* hardwareBuffer);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~HardwareBuffer(void);

	/// <summary>
	/// Lock the buffer for (potentially) reading / writing.
	/// </summary>
	/// <param name="offset">The byte offset from the start of the buffer to lock.</param>
	/// <param name="length">The size of the area to lock, in bytes.</param>
	/// <param name="options">Locking options.</param>
	/// <returns>A pointer to the locked buffer.</returns>
	void* lock(size_t offset, size_t length, LockOptions options);

	/// <summary>
	/// Lock the entire buffer for (potentially) reading / writing.
	/// </summary>
	/// <param name="options">Locking options</param>
	/// <returns>A pointer to the locked buffer.</returns>
	void* lock(LockOptions options);

	/// <summary>
	/// Releases the lock on this buffer.
	/// <para>
    /// Locking and unlocking a buffer can, in some rare circumstances such as
    /// switching video modes whilst the buffer is locked, corrupt the contents
    /// of a buffer. This is pretty rare, but if it occurs, this method will
    /// throw an exception, meaning you must re-upload the data. 
	/// </para>
	/// <para>
    /// Note that using the 'read' and 'write' forms of updating the buffer does
    /// not suffer from this problem, so if you want to be 100% sure your data
    /// will not be lost, use the 'read' and 'write' forms instead. 
	/// </para>
	/// </summary>
	void unlock();

	/// <summary>
	/// Reads data from the buffer and places it in the memory pointed to by pDest.
	/// </summary>
	/// <param name="offset">The byte offset from the start of the buffer to read.</param>
	/// <param name="length">The size of the area to read, in bytes.</param>
	/// <param name="dest">The area of memory in which to place the data, must be large enough to accommodate the data!</param>
	void readData(size_t offset, size_t length, void* dest);

	/// <summary>
	/// Writes data to the buffer from an area of system memory; note that you must ensure that your buffer is big enough.
	/// </summary>
	/// <param name="offset">The byte offset from the start of the buffer to start writing.</param>
	/// <param name="length">The size of the data to write to, in bytes.</param>
	/// <param name="source">The source of the data to be written.</param>
	void writeData(size_t offset, size_t length, void* source);

	/// <summary>
	/// Writes data to the buffer from an area of system memory; note that you must ensure that your buffer is big enough.
	/// </summary>
	/// <param name="offset">The byte offset from the start of the buffer to start writing.</param>
	/// <param name="length">The size of the data to write to, in bytes.</param>
	/// <param name="source">The source of the data to be written.</param>
	/// <param name="discardWholeBuffer">	If true, this allows the driver to discard the entire buffer when writing, such that DMA stalls can be avoided; use if you can.</param>
	void writeData(size_t offset, size_t length, void* source, bool discardWholeBuffer);

	/// <summary>
	/// Returns the size of this buffer in bytes.
	/// </summary>
	/// <returns>The size of the buffer in bytes.</returns>
	size_t getSizeInBytes();

	/// <summary>
	/// Returns the Usage flags with which this buffer was created.
	/// </summary>
	/// <returns>The usage flags of the buffer.</returns>
	Usage getUsage();

	/// <summary>
	/// Returns whether this buffer is held in system memory.
	/// </summary>
	/// <returns>True if the buffer is in system memory.</returns>
	bool isSystemMemory();

	/// <summary>
	/// Returns whether this buffer has a system memory shadow for quicker reading.
	/// </summary>
	/// <returns>True if there is a shadow buffer.</returns>
	bool hasShadowBuffer();

	/// <summary>
	/// Returns whether or not this buffer is currently locked.
	/// </summary>
	/// <returns>True if the buffer was locked.</returns>
	bool isLocked();

	/// <summary>
	/// Pass true to suppress hardware upload of shadow buffer changes. 
	/// </summary>
	/// <param name="suppress">True to suppress hardware upload of shadow buffer changes.</param>
	void suppressHardwareUpdate(bool suppress);
};

}