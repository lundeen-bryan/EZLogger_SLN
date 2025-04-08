# EZLogger v0.0.1

EZLogger is an Office Add-In developed using Visual Studio Tools for Office (VSTO) in VB.NET. The add-in integrates with Microsoft Word to provide various tools, including a report wizard, database operations, and task management.

## Requirements
To use the EZLogger add-in, you need the following:
- **Microsoft Word**: The add-in is designed to work with Word.
- **Microsoft Visual Studio 2022**: To compile and debug the add-in.
- **.NET Framework 4.7.2 or higher**: The project targets .NET Framework 4.7.2.
- **VSTO (Visual Studio Tools for Office)**: Required to run Office Add-ins.

## Setup Instructions

1. **Clone the Repository**

   To clone the repository to your local machine, use the following command:

   ```bash
   git clone https://github.com/lundeen-bryan/EZLogger_SLN.git
   ```

  > Alternatively you can download the zip file to your local machine.

2. **Open the Project in Visual Studio**

   - Open the solution file `EZLogger_SLN.sln` in Visual Studio 2022.
   - Ensure that you have the correct version of Office installed (e.g., Word) for debugging the add-in.

3. **Restore NuGet Packages**

   If you have any missing NuGet packages, restore them via Visual Studio:
   - Go to **Tools > NuGet Package Manager > Restore NuGet Packages**.

4. **Build the Project**

   - Press **F5** to build and run the project in Debug mode.
   - The add-in will load in Word (if set as the host application).

5. **Deploy the Add-In**

   If you're ready to deploy the add-in, package it with the necessary Office deployment tools, ensuring it meets your organization's guidelines.

## How to Use

After running the add-in:
- Open Microsoft Word.
- Go to the **EZ Logger** tab in the ribbon to access the various tools.
- Use the **Report Wizard** to generate reports or interact with the database options in the **Database Menu**.

## Forking or Cloning the Repository

To fork or clone the repository:

1. **Fork the Repository**:
   Click on the "Fork" button in the top-right corner of this GitHub page to create your own copy of the repository.

2. **Clone the Repository**:
   Once forked, you can clone the repository to your machine using the following command:

   ```bash
   git clone https://github.com/lundeen-bryan/EZLogger_SLN.git
   ```

3. **Make Changes**:
   You can now make changes to the code on your local machine. If you make any significant changes, feel free to submit a pull request back to the main repository.

## Versioning Strategy

Versioning follows these principles:

- **Development Phase (0.x.y)**:
  - Starting at v0.0.1
  - Minor version (0.x.0) increments with new features
  - Patch version (0.0.x) increments with bug fixes

- **Production Release (≥1.0.0)**:
  - v1.0.0 will mark our first stable release
  - Major version (x.0.0) increments with breaking changes
  - Minor version (0.x.0) increments with backward-compatible new features
  - Patch version (0.0.x) increments with backward-compatible bug fixes

#### Implementation Date

This versioning change was implemented on 2025-04-08 with the release of v0.0.1, replacing all previous versioning.

## Contributing

Contributions are welcome! If you'd like to contribute, please fork the repository, make your changes, and then submit a pull request with a detailed explanation of what you changed and why.

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Contact

If you have any questions or issues, feel free to reach out via GitHub issues or directly contact [lundeen-bryan@github].
