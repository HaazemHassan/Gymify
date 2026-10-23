# Gymify Copilot Instructions

## .NET Architecture Skill

For any work under `src/` or on the `Gymify.sln` solution, first load and follow the project skill:

- [`.agents/skills/dotnet-architecture/SKILL.md`](../.agents/skills/dotnet-architecture/SKILL.md)

Use the supporting references linked from that skill when the task involves architecture rules, CQRS, specifications, validation, AutoMapper projections, caching, security, or feature workflows. Treat the skill's mandatory rules and quality checklist as project requirements.

## Verification

After code changes, run:

```powershell
dotnet build Gymify.sln
```

Preserve the existing Clean Architecture boundaries and established project patterns. Do not add a second copy of the architecture skill under `.github`; `.agents/skills/dotnet-architecture` is the single source of truth.
