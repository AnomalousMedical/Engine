#pragma once

#include <vector>
#include <list>

namespace SoundWrapper
{

class Source;

using namespace std;

class SourceManager
{
private:
	vector<Source*> sources;
	list<Source*> playingSources;
	vector<Source*> removedSources; //Used when calling the update function so the iterator does not break.
	vector<Source*> addedSources; //Used when calling the update function so the iterator does not break.
	bool inUpdateIterLoop;

public:
	SourceManager(void);

	~SourceManager(void);

	Source* getPooledSource();

	//Internal, do not create wrappers
	
	//Only call from source
	void _addPlayingSource(Source* source);

	//Only call from source
	void _removePlayingSource(Source* source);

	//Only call from source
	void _addSourceToPool(Source* source);

	//Only call from OpenALManager
	void _update();
};

}