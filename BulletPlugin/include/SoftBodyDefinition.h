#pragma once

#include "BulletElementDefinition.h"
#include "AutoPtr.h"

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;
using namespace Engine::Attributes;

struct btSoftBody::Config;

namespace BulletPlugin
{

//Enums
[Engine::Attributes::SingleEnum]
enum class SoftBodyCollision
{
	RVSmask	=	0x000f,	///Rigid versus soft mask
	SDF_RS	=	0x0001,	///SDF based rigid vs soft
	CL_RS	=	0x0002, ///Cluster vs convex rigid vs soft

	SVSmask	=	0x0030,	///Rigid versus soft mask		
	VF_SS	=	0x0010,	///Vertex vs face soft vs soft handling
	CL_SS	=	0x0020, ///Cluster vs cluster soft vs soft handling
	CL_SELF =	0x0040, ///Cluster soft body self collision
	/* presets	*/ 
	Default	=	SDF_RS,
	END
};

[Engine::Attributes::SingleEnum]
enum class SoftBodyAeroModel 
{
	V_Point,	///Vertex normals are oriented toward velocity
	V_TwoSided,	///Vertex normals are fliped to match velocity	
	V_OneSided,	///Vertex normals are taken as it is	
	F_TwoSided,	///Face normals are fliped to match velocity
	F_OneSided,	///Face normals are taken as it is
	END
};

public ref class SoftBodyDefinition : public BulletElementDefinition
{
private:
	EditInterface^ editInterface;
	AutoPtr<btSoftBody::Config> config;
	AutoPtr<btSoftBody::Material> material;
	float mass;
	bool massFromFaces;
	bool randomizeConstraints;
	String^ softBodyProviderName;
	float collisionMargin;

	static MemberScanner^ memberScanner = gcnew MemberScanner();

internal:
	/// <summary>
    /// Create a new element normally as a part of scene and add it to instance.
    /// </summary>
    /// <param name="instance">The SimObject to add the element to.</param>
    /// <param name="scene">The PhysSceneManager to create the product with.</param>
    virtual void createProduct(SimObjectBase^ instance, BulletScene^ scene) override;

    /// <summary>
    /// Create a new element staticly as a part of scene and add it to instance.
    /// </summary>
    /// <param name="instance">The SimObject to add the element to.</param>
    /// <param name="scene">The PhysSceneManager to create the product with.</param>
    virtual void createStaticProduct(SimObjectBase^ instance, BulletScene^ scene) override;

	static SimElementDefinition^ Create(String^ name, EditUICallback^ callback)
	{
		return gcnew SoftBodyDefinition(name);
	}

	property btSoftBody::Config* sbConfig
	{
		btSoftBody::Config* get()
		{
			return config.Get();
		}
	}

	property btSoftBody::Material* DefaultMaterial
	{
		btSoftBody::Material* get()
		{
			return material.Get();
		}
	}

public:
	static SoftBodyDefinition()
    {
        memberScanner->ProcessFields = false;
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }

	SoftBodyDefinition(String^ name);

	virtual ~SoftBodyDefinition(void);

	virtual void registerScene(SimSubScene^ subscene, SimObjectBase^ instance) override;

	virtual EditInterface^ getEditInterface() override;

//Properties
//Configuration
[Editable]
property SoftBodyAeroModel AeroModel
{
	SoftBodyAeroModel get()
	{
		return static_cast<SoftBodyAeroModel>(config->aeromodel);
	}
	
	void set(SoftBodyAeroModel value)
	{
		config->aeromodel = static_cast<btSoftBody::eAeroModel::_>(value);
	}
}

// Velocities correction factor (Baumgarte)
[Editable]
property float kVCF
{
	float get()
	{
		return config->kVCF;
	}
	
	void set(float value)
	{
		config->kVCF = value;
	}
}

// Damping coefficient [0,1]
[Editable]
property float kDP
{
	float get()
	{
		return config->kDP;
	}
	
	void set(float value)
	{
		config->kDP = value;
	}
}

// Drag coefficient [0,+inf]
[Editable]
property float kDG
{
	float get()
	{
		return config->kDG;
	}
	
	void set(float value)
	{
		config->kDG = value;
	}
}

// Lift coefficient [0,+inf]
[Editable]
property float kLF
{
	float get()
	{
		return config->kLF;
	}
	
	void set(float value)
	{
		config->kLF = value;
	}
}

// Pressure coefficient [-inf,+inf]
[Editable]
property float kPR
{
	float get()
	{
		return config->kPR;
	}
	
	void set(float value)
	{
		config->kPR = value;
	}
}

// Volume conversation coefficient [0,+inf]
[Editable]
property float kVC
{
	float get()
	{
		return config->kVC;
	}
	
	void set(float value)
	{
		config->kVC = value;
	}
}

// Dynamic friction coefficient [0,1]
[Editable]
property float kDF
{
	float get()
	{
		return config->kDF;
	}
	
	void set(float value)
	{
		config->kDF = value;
	}
}

// Pose matching coefficient [0,1]		
[Editable]
property float kMT
{
	float get()
	{
		return config->kMT;
	}
	
	void set(float value)
	{
		config->kMT = value;
	}
}

// Rigid contacts hardness [0,1]
[Editable]
property float kCHR
{
	float get()
	{
		return config->kCHR;
	}
	
	void set(float value)
	{
		config->kCHR = value;
	}
}

// Kinetic contacts hardness [0,1]
[Editable]
property float kKHR
{
	float get()
	{
		return config->kKHR;
	}
	
	void set(float value)
	{
		config->kKHR = value;
	}
}

// Soft contacts hardness [0,1]
[Editable]
property float kSHR
{
	float get()
	{
		return config->kSHR;
	}
	
	void set(float value)
	{
		config->kSHR = value;
	}
}

// Anchors hardness [0,1]
[Editable]
property float kAHR
{
	float get()
	{
		return config->kAHR;
	}
	
	void set(float value)
	{
		config->kAHR = value;
	}
}

// Soft vs rigid hardness [0,1] (cluster only)
[Editable]
property float kSRHR_CL
{
	float get()
	{
		return config->kSRHR_CL;
	}
	
	void set(float value)
	{
		config->kSRHR_CL = value;
	}
}

// Soft vs kinetic hardness [0,1] (cluster only)
[Editable]
property float kSKHR_CL
{
	float get()
	{
		return config->kSKHR_CL;
	}
	
	void set(float value)
	{
		config->kSKHR_CL = value;
	}
}

// Soft vs soft hardness [0,1] (cluster only)
[Editable]
property float kSSHR_CL
{
	float get()
	{
		return config->kSSHR_CL;
	}
	
	void set(float value)
	{
		config->kSSHR_CL = value;
	}
}

// Soft vs rigid impulse split [0,1] (cluster only)
[Editable]
property float kSR_SPLT_CL
{
	float get()
	{
		return config->kSR_SPLT_CL;
	}
	
	void set(float value)
	{
		config->kSR_SPLT_CL = value;
	}
}

// Soft vs rigid impulse split [0,1] (cluster only)
[Editable]
property float kSK_SPLT_CL
{
	float get()
	{
		return config->kSK_SPLT_CL;
	}
	
	void set(float value)
	{
		config->kSK_SPLT_CL = value;
	}
}

// Soft vs rigid impulse split [0,1] (cluster only)
[Editable]
property float kSS_SPLT_CL
{
	float get()
	{
		return config->kSS_SPLT_CL;
	}
	
	void set(float value)
	{
		config->kSS_SPLT_CL = value;
	}
}

// Maximum volume ratio for pose
[Editable]
property float maxvolume
{
	float get()
	{
		return config->maxvolume;
	}
	
	void set(float value)
	{
		config->maxvolume = value;
	}
}

// Time scale
[Editable]
property float timescale
{
	float get()
	{
		return config->timescale;
	}
	
	void set(float value)
	{
		config->timescale = value;
	}
}

// Velocities solver iterations
[Editable]
property int	viterations
{
	int get()
	{
		return config->viterations;
	}
	
	void set(int value)
	{
		config->viterations = value;
	}
}

// Positions solver iterations
[Editable]
property int	piterations
{
	int get()
	{
		return config->piterations;
	}
	
	void set(int value)
	{
		config->piterations = value;
	}
}

// Drift solver iterations
[Editable]
property int	diterations
{
	int get()
	{
		return config->diterations;
	}
	
	void set(int value)
	{
		config->diterations = value;
	}
}

// Cluster solver iterations
[Editable]
property int citerations
{
	int get()
	{
		return config->citerations;
	}
	
	void set(int value)
	{
		config->citerations = value;
	}
}

// Collisions flags
[Editable]
property SoftBodyCollision	Collisions
{
	SoftBodyCollision get()
	{
		return static_cast<SoftBodyCollision>(config->collisions);
	}
	
	void set(SoftBodyCollision value)
	{
		config->collisions = static_cast<btSoftBody::fCollision::_>(value);
	}
}

//Default material
[Editable]
property float DefMat_kLST
{
	float get()
	{
		return material->m_kLST;
	}
	
	void set(float value)
	{
		material->m_kLST = value;
	}
}

[Editable]
property float DefMat_kAST
{
	float get()
	{
		return material->m_kAST;
	}
	
	void set(float value)
	{
		material->m_kAST = value;
	}
}

[Editable]
property float DefMat_kVST
{
	float get()
	{
		return material->m_kVST;
	}
	
	void set(float value)
	{
		material->m_kVST = value;
	}
}

[Editable]
property float CollisionMargin
{
	float get()
	{
		return collisionMargin;
	}
	
	void set(float value)
	{
		collisionMargin = value;
	}
}

[Editable]
property float Mass
{
	float get()
	{
		return mass;
	}
	
	void set(float value)
	{
		mass = value;
	}
}

[Editable]
property bool MassFromFaces
{
	bool get()
	{
		return massFromFaces;
	}
	
	void set(bool value)
	{
		massFromFaces = value;
	}
}

[Editable]
property bool RandomizeConstraints
{
	bool get()
	{
		return randomizeConstraints;
	}
	
	void set(bool value)
	{
		randomizeConstraints = value;
	}
}

[Editable]
property String^ SoftBodyProviderName
{
	String^ get()
	{
		return softBodyProviderName;
	}
	
	void set(String^ value)
	{
		softBodyProviderName = value;
	}
}

//Saving

protected:
	SoftBodyDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info) override;

//End Saving
};

}