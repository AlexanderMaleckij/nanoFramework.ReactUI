# nanoFramework.ReactUI

## Solution Projects

### reactproject
This project is created from the standard Visual Studio template named "Standalone TypeScript React Project" with several minor configuration adjustments:
- `vite.config.ts`
  - Minification has been enabled.
  - The `vite-plugin-compression` plugin has been added to compress the build results, allowing for more efficient space usage.
- `package.json`
  - The `build` script has been modified to disable colorful output, which is not supported by the Visual Studio output window.
  - A `clean` script has been added to remove build results.

### nanoframework.ReactUI
This project depends on the build of `reactproject`. It leverages two 3rd party NuGet packages:
- [amaletski.nanoFramework.MSBuildTasks](https://github.com/AlexanderMaleckij/nanoFramework.MSBuildTasks)
- [amaletski.nanoFramework.SourceGenerators](https://github.com/AlexanderMaleckij/nanoFramework.SourceGenerators)

The first package includes a task that generates a `.resx` file from the `reactproject` build result files and embeds `.resources`.

The second package analyzes the generated `.resx` file and produces the `ResourcesMetadataProvider` class, which is utilized by the `ResourcesServer` class.

Due to the complexity of installing, updating, and configuring these packages, it is strongly recommended to consult their READMEs for guidance.

### nanoframework.ReactUI.Consumer
This project references `nanoframework.ReactUI`. Essentially, it serves as a straightforward method to test `nanoframework.ReactUI` by setting up a WiFi AP and initiating the resources server.
