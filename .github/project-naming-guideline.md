# Project Naming Guideline

This guideline defines naming conventions for files, classes, interfaces, selectors, and variables in this monorepo, based on current workspace structure and best practices for Angular and Nest.js projects.

## 1. File and Folder Naming
- Use **kebab-case** for all file and directory names.
- Group related features in dedicated folders (feature modules).
- Use barrels (`index.ts`) for cleaner imports where appropriate.

### Angular
- Component: `feature-name.component.ts`
- Service: `feature-name.service.ts`
- Module: `feature-name.module.ts`
- Routing: `feature-name.routes.ts`
- Selector: `app-feature-name`
- Test: `feature-name.component.spec.ts`, `feature-name.service.spec.ts`

### Nest.js
- Controller: `feature-name.controller.ts`
- Service: `feature-name.service.ts`
- Module: `feature-name.module.ts`
- DTO: `create-feature-name.dto.ts`, `update-feature-name.dto.ts`
- Test: `feature-name.controller.spec.ts`, `feature-name.service.spec.ts`

## 2. Class and Interface Naming
- Use **PascalCase** for all class and interface names.
- Angular component: `FeatureNameComponent`
- Angular service: `FeatureNameService`
- Angular module: `FeatureNameModule`
- Nest.js controller: `FeatureNameController`
- Nest.js service: `FeatureNameService`
- Nest.js module: `FeatureNameModule`
- Interface: `IFeatureName` or `FeatureNameInterface`
- DTO: `CreateFeatureNameDto`, `UpdateFeatureNameDto`

## 3. Variable and Function Naming
- Use **camelCase** for variables, functions, and methods.
- Use verbs for functions (e.g., `getInvoice`, `createUser`).
- Use `isX`, `hasX`, `canX` for booleans.

## 4. Selector Naming (Angular)
- Use `app-` prefix for all component selectors: `app-feature-name`

## 5. Environment Variables
- Use **UPPERCASE** with underscores: `API_URL`, `DB_HOST`

## 6. General Principles
- Avoid abbreviations except for well-known terms (API, URL, DTO, etc.).
- Keep names descriptive and consistent across the codebase.
- Follow the Angular and Nest.js style guides for any cases not covered here.
