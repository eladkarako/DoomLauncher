namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    public interface IStatisticsReader
    {
        event NewStatisticsEventHandler NewStastics;

        void ReadNow();
        void Start();
        void Stop();

        string[] Errors { get; }

        IGameFileDataSource GameFile { get; set; }

        string LaunchParameter { get; }

        bool ReadOnClose { get; }
    }
}

