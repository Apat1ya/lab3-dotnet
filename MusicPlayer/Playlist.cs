namespace MusicPlayer
{
    public class TrackStartedEventArgs<T> : EventArgs
    {
        public T Track { get; }
        public int Position { get; }

        public TrackStartedEventArgs(T track, int position)
        {
            Track = track;
            Position = position;
        }
    }

    public class Playlist<T> where T : IPlayable
    {
        private readonly List<T> _tracks = new();
        private Func<int, int, int>? _nextTrackRule;

        public event EventHandler<TrackStartedEventArgs<T>>? TrackStarted;

        public int Count => _tracks.Count;
        public int CurrentPosition { get; private set; }

        public void Add(T track) => _tracks.Add(track);

        public void Next()
        {
            if (_tracks.Count == 0)
                return;

            CurrentPosition = _nextTrackRule?.Invoke(CurrentPosition, _tracks.Count)
                              ?? (CurrentPosition + 1) % _tracks.Count;
            OnTrackStarted(new TrackStartedEventArgs<T>(_tracks[CurrentPosition], CurrentPosition));
        }

        public int GetTotalDuration() => _tracks.Sum(t => t.DurationSeconds);

        public void ForEach(Action<T> action)
        {
            foreach (T track in _tracks)
                action(track);
        }

        public void SetNextTrackRule(Func<int, int, int>? rule) => _nextTrackRule = rule;

        protected virtual void OnTrackStarted(TrackStartedEventArgs<T> e) => TrackStarted?.Invoke(this, e);
    }
}
