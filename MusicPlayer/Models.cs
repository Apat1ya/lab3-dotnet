namespace MusicPlayer
{
    public interface IPlayable
    {
        string Title { get; }
        int DurationSeconds { get; }
    }

    public class Song : IPlayable
    {
        public string Title { get; }
        public string Artist { get; }
        public int DurationSeconds { get; }

        public Song(string title, string artist, int durationSeconds)
        {
            Title = title;
            Artist = artist;
            DurationSeconds = durationSeconds;
        }

        public override string ToString() => $"{Artist} — {Title} ({DurationSeconds / 60}:{DurationSeconds % 60:00})";
    }
}
