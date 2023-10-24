# Disk Usage Tool

Author: Andrew Photinakis

## Description

This tool calculates disk usage in a specified directory, summarizing the number of folders, files, and total size. It offers both sequential and parallel modes for processing, providing users with flexibility based on their system capabilities and preferences.

## Features

- **Sequential Mode**: Processes directories and files sequentially, measuring disk usage.
- **Parallel Mode**: Utilizes parallel processing to optimize performance on multi-core systems.
- **Combined Mode**: Runs parallel mode followed by sequential mode for comprehensive analysis.

## Usage

To use the Disk Usage Tool, follow the instructions below:

### Prerequisites

- .NET Core SDK installed

### Installation

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/AndrewP987/DiskUsage.git
   ```

2. Navigate to the project directory:

   ```bash
   cd disk-usage-tool
   ```

### Run the Tool

The tool accepts the following parameters:

- **-s**: Run in single-threaded mode
- **-d**: Run in parallel mode (uses all available processors)
- **-b**: Run in both parallel and single-threaded mode. Runs parallel mode followed by sequential mode.

Example usage:

```bash
dotnet run -d /path/to/analyze
```

### Build from Source

To build the project, run the following command:

```bash
dotnet build
```

### License

This project is licensed under the [MIT License](LICENSE).