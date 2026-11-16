using System.Text;

namespace MusicPlayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            var playlist = new Playlist<Song>();
            playlist.Add(new Song("Stressed Out", "Twenty One Pilots", 202));
            playlist.Add(new Song("Believer", "Imagine Dragons", 204));
            playlist.Add(new Song("Numb", "Linkin Park", 187));
            playlist.Add(new Song("Shape of You", "Ed Sheeran", 233));

            var history = new List<Song>();

            playlist.TrackStarted += PrintNowPlaying;
            playlist.TrackStarted += (sender, e) => history.Add(e.Track);

            bool running = true;
            while (running)
            {
                PrintMenu();
                int choice = ReadInt("Ваш вибір: ", 1, 6);
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        running = false;
                        Console.WriteLine("Роботу завершено.");
                        break;
                    case 2:
                        AddSong(playlist);
                        break;
                    case 3:
                        ShowPlaylist(playlist);
                        break;
                    case 4:
                        playlist.Next();
                        break;
                    case 5:
                        ChooseMode(playlist);
                        break;
                    case 6:
                        ShowHistory(history);
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void PrintNowPlaying(object? sender, TrackStartedEventArgs<Song> e)
        {
            Console.WriteLine($"Зараз грає: {e.Position + 1}. {e.Track}");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("===== Музичний плейлист =====");
            Console.WriteLine("1. Вихід");
            Console.WriteLine("2. Додати трек");
            Console.WriteLine("3. Показати плейлист");
            Console.WriteLine("4. Наступний трек");
            Console.WriteLine("5. Обрати режим");
            Console.WriteLine("6. Показати історію прослуховування");
        }

        private static void AddSong(Playlist<Song> playlist)
        {
            string title = ReadNonEmpty("Назва треку: ");
            string artist = ReadNonEmpty("Виконавець: ");
            int duration = ReadInt("Тривалість, с: ", 1, 3600);
            playlist.Add(new Song(title, artist, duration));
            Console.WriteLine($"Трек \"{title}\" додано.");
        }

        private static void ShowPlaylist(Playlist<Song> playlist)
        {
            if (playlist.Count == 0)
            {
                Console.WriteLine("Плейлист порожній.");
                return;
            }

            int index = 0;
            playlist.ForEach(s =>
            {
                string marker = index == playlist.CurrentPosition ? ">" : " ";
                Console.WriteLine($"{marker} {index + 1}. {s}");
                index++;
            });

            int total = playlist.GetTotalDuration();
            Console.WriteLine($"Загальна тривалість: {total / 60}:{total % 60:00}");
        }

        private static void ChooseMode(Playlist<Song> playlist)
        {
            Console.WriteLine("1. По порядку");
            Console.WriteLine("2. Повтор треку");
            Console.WriteLine("3. Випадковий");
            Console.WriteLine("4. У зворотному порядку");
            int mode = ReadInt("Режим: ", 1, 4);

            Func<int, int, int>? rule = mode switch
            {
                2 => (current, count) => current,
                3 => (current, count) => Random.Shared.Next(count),
                4 => (current, count) => (current - 1 + count) % count,
                _ => null
            };
            playlist.SetNextTrackRule(rule);
            Console.WriteLine("Режим змінено.");
        }

        private static void ShowHistory(List<Song> history)
        {
            if (history.Count == 0)
            {
                Console.WriteLine("Історія порожня.");
                return;
            }

            Console.WriteLine("Історія прослуховування:");
            for (int i = 0; i < history.Count; i++)
                Console.WriteLine($"  {i + 1}. {history[i]}");
        }

        private static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input))
                    return input;
                Console.WriteLine("Значення не може бути порожнім.");
            }
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Введіть ціле число від {min} до {max}.");
            }
        }
    }
}
