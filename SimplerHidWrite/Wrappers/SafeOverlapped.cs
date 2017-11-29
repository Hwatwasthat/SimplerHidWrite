using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Threading;

using PInvoke;

namespace IliumVR.Tools.SimplerHidWrite.Wrappers
{
	public class SafeOverlapped : IDisposable
	{
		private Kernel32.OVERLAPPED overlapped;
		private SafeWaitHandle waitHandle;
		private GCHandle pinnedHandle;

		private bool disposed = false;

		public SafeOverlapped()
		{
			pinnedHandle = GCHandle.Alloc(overlapped, GCHandleType.Pinned);
		}

		public SafeOverlapped(WaitHandle wait)
			: this()
		{
			this.waitHandle = wait.SafeWaitHandle;
		}

		~SafeOverlapped()
		{
			Dispose(false);
		}

		public bool IsDisposed
		{
			get
			{
				return disposed;
			}
		}

		public IntPtr InternalLow
		{
			get { return overlapped.Internal; }
			set { overlapped.Internal = value; }
		}

		public IntPtr InternalHigh
		{
			get { return overlapped.InternalHigh; }
			set { overlapped.InternalHigh = value; }
		}

		public int OffsetLow
		{
			get { return overlapped.Offset; }
			set { overlapped.Offset = value; }
		}

		public int OffsetHigh
		{
			get { return overlapped.OffsetHigh; }
			set { overlapped.OffsetHigh = value; }
		}

		public SafeWaitHandle EventHandle
		{
			get { return waitHandle; }
			set { overlapped.hEvent = value.DangerousGetHandle(); waitHandle = value; }
		}

		public IntPtr PinnedHandle
		{
			get { return pinnedHandle.AddrOfPinnedObject(); }
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
				}

				pinnedHandle.Free();
				waitHandle = null;
				overlapped.hEvent = IntPtr.Zero;

				disposed = true;
			}
		}
	}
}
