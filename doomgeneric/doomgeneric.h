//
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

#ifndef DOOM_GENERIC
#define DOOM_GENERIC

#include <stdlib.h>
#include <stdint.h>

#define DOOMGENERIC_RESX 320
#define DOOMGENERIC_RESY 200

#ifdef _WIN32
#define DLL_EXPORT __declspec(dllexport)
#else
#define DLL_EXPORT
#endif

extern uint32_t DG_ScreenBuffer[DOOMGENERIC_RESX * DOOMGENERIC_RESY];
extern char *DG_ConfigDir;

void doomgeneric_create(const char *dir,
	void (*pDG_DrawFrame)(unsigned char *),
	uint32_t (*pDG_GetTicksMs)(void),
	int (*pDG_GetKey)(int*, unsigned char *),
	void (*pDG_Exit)(void));

extern void (*DG_DrawFrame)(unsigned char *);
extern uint32_t (*DG_GetTicksMs)(void);
extern int (*DG_GetKey)(int*, unsigned char *);
extern void (*DG_Exit)(void);

#endif /* DOOM_GENERIC */
