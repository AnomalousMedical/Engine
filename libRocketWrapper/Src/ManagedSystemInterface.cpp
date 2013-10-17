#include "StdAfx.h"
#include "ManagedSystemInterface.h"

ManagedSystemInterface::ManagedSystemInterface(GetElapsedTimeDelegate etDelegate, LogMessageDelegate logDelegate)
	:etDelegate(etDelegate),
	logDelegate(logDelegate)
{
}

ManagedSystemInterface::~ManagedSystemInterface()
{
}

float ManagedSystemInterface::GetElapsedTime()
{
	return etDelegate();
}

bool ManagedSystemInterface::LogMessage(Rocket::Core::Log::Type type, const Rocket::Core::String& message)
{
	logDelegate(type, message.CString());
	return false;
}

void ManagedSystemInterface::JoinPath(Rocket::Core::String& translated_path, const Rocket::Core::String& document_path, const Rocket::Core::String& path)
{
	//A leading / means the path is a rooted path, so try to find its matching root path based off the document name.
	if(path.Substring(0, 1) == "/")
	{
		for (std::list<Rocket::Core::String>::iterator it = rootedPaths.begin(); it != rootedPaths.end(); ++it)
		{
			Rocket::Core::String currentRootPath = *it;
			if(document_path.Length() > currentRootPath.Length())
			{
				if(strncasecmp(document_path.CString(), currentRootPath.CString(), currentRootPath.Length()) == 0)
				{
					//Use the current root path as the translated path
					translated_path = currentRootPath;

					// Append the paths and send through URL to removing any '..'.
					Rocket::Core::URL url(translated_path.Replace(":", "|") + path.Replace("\\", "/"));
					translated_path = url.GetPathedFileName().Replace("|", ":");

					return;
				}
			}
			//Try to match the root path without the protocol. Right now this is basically hardcoded for file:/// (8 chars)
			size_t noProtocolLength = currentRootPath.Length() - 8;
			if(document_path.Length() > noProtocolLength)
			{
				if(strncasecmp(document_path.CString(), currentRootPath.CString() + 8, noProtocolLength) == 0)
				{
					//Use the current root path as the translated path
					translated_path = currentRootPath.Substring(8);

					// Append the paths and send through URL to removing any '..'.
					Rocket::Core::URL url(translated_path.Replace(":", "|") + path.Replace("\\", "/"));
					translated_path = url.GetPathedFileName().Replace("|", ":");

					return;
				}
			}
		}
	}

	// If the path is absolute, strip the leading ~/ and return it.
	if (path.Substring(0, 2) == "~/")
	{
		translated_path = path.Substring(2);
		return;
	}

	// If the path is a Windows-style absolute path, return it directly.
	size_t drive_pos = path.Find(":");
	size_t slash_pos = Rocket::Core::Math::Min(path.Find("/"), path.Find("\\"));
	if (drive_pos != Rocket::Core::String::npos &&
		drive_pos < slash_pos)
	{
		translated_path = path;
		return;
	}

	// Strip off the referencing document name.
	translated_path = document_path;
	translated_path = translated_path.Replace("\\", "/");
	size_t file_start = translated_path.RFind("/");
	if (file_start != Rocket::Core::String::npos)
		translated_path.Resize(file_start + 1);
	else
		translated_path.Clear();

	// Append the paths and send through URL to removing any '..'.
	Rocket::Core::URL url(translated_path.Replace(":", "|") + path.Replace("\\", "/"));
	translated_path = url.GetPathedFileName().Replace("|", ":");
}

void ManagedSystemInterface::AddRootPath(const Rocket::Core::String& rootPath)
{
	rootedPaths.push_back(rootPath);
}

void ManagedSystemInterface::RemoveRootPath(const Rocket::Core::String& rootPath)
{
	rootedPaths.remove(rootPath);
}

extern "C" _AnomalousExport ManagedSystemInterface* ManagedSystemInterface_Create(GetElapsedTimeDelegate etDelegate, LogMessageDelegate logDelegate)
{
	return new ManagedSystemInterface(etDelegate, logDelegate);
}

extern "C" _AnomalousExport void ManagedSystemInterface_Delete(ManagedSystemInterface* systemInterface)
{
	delete systemInterface;
}

extern "C" _AnomalousExport void SystemInterface_JoinPath(Rocket::Core::SystemInterface* systemInterface, String documentPath, String path, StringRetrieverCallback stringCallback)
{
	Rocket::Core::String result;
	systemInterface->JoinPath(result, documentPath, path);
	stringCallback(result.CString());
}

extern "C" _AnomalousExport void ManagedSystemInterface_AddRootPath(ManagedSystemInterface* systemInterface, String rootPath)
{
	systemInterface->AddRootPath(rootPath);
}

extern "C" _AnomalousExport void ManagedSystemInterface_RemoveRootPath(ManagedSystemInterface* systemInterface, String rootPath)
{
	systemInterface->RemoveRootPath(rootPath);
}