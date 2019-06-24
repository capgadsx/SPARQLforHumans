﻿namespace SparqlForHumans.Lucene.Indexing
{
    public abstract class BaseNotifier
    {
        private readonly NLog.Logger Logger = SparqlForHumans.Logger.Logger.Init();
        public int NotifyTicks { get; } = 100000;
        public abstract string NotifyMessage { get; }

        public virtual void LogProgress(long Ticks, bool overrideCheck = false)
        {
            if (Ticks % NotifyTicks == 0 || overrideCheck)
                Logger.Info($"{NotifyMessage}, Count: {Ticks:N0}");
        }
    }
}