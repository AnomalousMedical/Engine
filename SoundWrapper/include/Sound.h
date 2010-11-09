#pragma once

class Source;

class Sound
{
public:
	Sound(void);

	virtual ~Sound(void);

	virtual void close() = 0;

	virtual bool enqueueSource(Source* source) = 0;
};
