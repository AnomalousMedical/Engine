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

public:
	SourceManager(void);

	~SourceManager(void);

	Source* getPooledSource();

	//Internal, do not create wrappers
	
	//Only call from source
	void _addPlayingSource(Source* source);

	//Only call from source
	void _removePlayingSource(Source* source);

	//Only call from OpenALManager
	void _update();
};

}