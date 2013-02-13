#pragma once

namespace SoundWrapper
{

class Stream;

class OggEncoder
{
public:
	OggEncoder(void);

	virtual ~OggEncoder(void);

	bool encodeToStream(Stream* source, Stream* destination);
};

}