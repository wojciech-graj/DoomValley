//
// Copyright(C) 1993-1996 Id Software, Inc.
// Copyright(C) 2005-2014 Simon Howard
// Copyright(C) 2024 Wojciech Graj
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// DESCRIPTION:
//	Nil.
//

#include "doomgeneric.h"

#include <stdio.h>
#include <string.h>

#include "m_argv.h"
#include "d_iwad.h"

void D_DoomMain (void);
void M_FindResponseFile(void);

uint32_t DG_ScreenBuffer[DOOMGENERIC_RESX * DOOMGENERIC_RESY];
char *DG_ConfigDir;

void (*DG_DrawFrame)(unsigned char *);
uint32_t (*DG_GetTicksMs)(void);
int (*DG_GetKey)(int*, unsigned char*);
void (*DG_Exit)(void);

void DLL_EXPORT doomgeneric_Create(const char *const dir,
	void (*pDG_DrawFrame)(unsigned char *),
	uint32_t (*pDG_GetTicksMs)(),
	int (*pDG_GetKey)(int*, unsigned char*),
	void (*pDG_Exit)(void))
{
	myargc = 0;
	myargv = NULL;

	DG_ConfigDir = strdup(dir);
	AddIWADDir(DG_ConfigDir);

	DG_DrawFrame = pDG_DrawFrame;
	DG_GetTicksMs = pDG_GetTicksMs;
	DG_GetKey = pDG_GetKey;
	DG_Exit = pDG_Exit;

	M_FindResponseFile();

	D_DoomMain ();
}
