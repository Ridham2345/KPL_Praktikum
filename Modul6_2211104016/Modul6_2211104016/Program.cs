using System;
using System.Collections.Generic;

namespace Modul6_2211104016
{
    public class SayaTubeVideo
    {
        private int id;
        private string title;
        private int playCount;

        public SayaTubeVideo(string title)
        {
            if (title == null) throw new ArgumentNullException("Judul video tidak boleh null.");
            if (title.Length > 200) throw new ArgumentException("Judul video maksimal 200 karakter.");

            Random random = new Random();
            this.id = random.Next(10000, 99999);
            this.title = title;
            this.playCount = 0;
        }

        public void IncreasePlayCount(int count)
        {
            if (count < 0) throw new ArgumentException("Play count tidak boleh negatif.");
            if (count > 25000000) throw new ArgumentException("Penambahan play count maksimal 25.000.000 per panggilan.");

            try
            {
                checked
                {
                    playCount += count;
                }
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void PrintVideoDetails()
        {
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Play Count: {playCount}");
        }

        public int GetPlayCount()
        {
            return playCount;
        }

        public string GetTitle()
        {
            return title;
        }
    }

    public class SayaTubeUser
    {
        private int id;
        private string username;
        private List<SayaTubeVideo> uploadedVideos;

        public SayaTubeUser(string username)
        {
            // Precondition v & vi: Username tidak null dan maksimal 100 karakter
            if (username == null) throw new ArgumentNullException("Username tidak boleh null.");
            if (username.Length > 100) throw new ArgumentException("Username maksimal 100 karakter.");

            Random random = new Random();
            this.id = random.Next(10000, 99999);
            this.username = username;
            this.uploadedVideos = new List<SayaTubeVideo>();
        }

        public void AddVideo(SayaTubeVideo video)
        {
            // Precondition vii & viii: Video tidak null dan playCount < int.MaxValue
            if (video == null) throw new ArgumentNullException("Video tidak boleh null.");
            if (video.GetPlayCount() >= int.MaxValue) throw new ArgumentException("Play count video melebihi batas maksimum integer.");

            uploadedVideos.Add(video);
        }

        public int GetTotalVideoPlayCount()
        {
            int totalPlayCount = 0;
            foreach (var video in uploadedVideos)
            {
                totalPlayCount += video.GetPlayCount();
            }
            return totalPlayCount;
        }

        public void PrintAllVideoPlaycount()
        {
            Console.WriteLine($"User: {username}");
            // Postcondition: Maksimal 8 video yang dicetak
            int maxVideosToPrint = Math.Min(uploadedVideos.Count, 8);
            for (int i = 0; i < maxVideosToPrint; i++)
            {
                Console.WriteLine($"Video {i + 1} judul: {uploadedVideos[i].GetTitle()}");
            }
            if (uploadedVideos.Count > 8)
                Console.WriteLine("...dan lainnya (maksimal 8 video ditampilkan).");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SayaTubeUser user = new SayaTubeUser("Arxc Bandit");

                string[] hackerMovies = {
                    "The Matrix", "Hackers", "WarGames", "Sneakers", "Swordfish",
                    "Blackhat", "The Girl with the Dragon Tattoo", "Mr.Robot", "Tron", "Who Am I"
                };

                foreach (string movie in hackerMovies)
                {
                    SayaTubeVideo video = new SayaTubeVideo($"Review Film {movie} oleh Arxc Bandit");
                    video.IncreasePlayCount(new Random().Next(100, 1000));
                    user.AddVideo(video);
                }

                Console.WriteLine("=== Output Utama ===");
                user.PrintAllVideoPlaycount();
                Console.WriteLine($"Total Play Count: {user.GetTotalVideoPlayCount()}");

                Console.WriteLine("\n=== Pengujian Design by Contract ===");

                Console.WriteLine("\nUji precondition judul > 200 karakter:");
                try
                {
                    string longTitle = new string('A', 201);
                    SayaTubeVideo invalidVideo = new SayaTubeVideo(longTitle);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nUji precondition judul null:");
                try
                {
                    SayaTubeVideo nullVideo = new SayaTubeVideo(null);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                SayaTubeVideo testVideo = new SayaTubeVideo("Test Video");
                Console.WriteLine("\nUji precondition play count > 25 juta:");
                try
                {
                    testVideo.IncreasePlayCount(26000000);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nUji precondition play count negatif:");
                try
                {
                    testVideo.IncreasePlayCount(-100);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nUji precondition username > 100 karakter:");
                try
                {
                    string longUsername = new string('B', 101);
                    SayaTubeUser invalidUser = new SayaTubeUser(longUsername);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nUji precondition username null:");
                try
                {
                    SayaTubeUser nullUser = new SayaTubeUser(null);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nUji precondition video null:");
                try
                {
                    user.AddVideo(null);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nUji precondition playCount >= int.MaxValue:");
                SayaTubeVideo maxPlayVideo = new SayaTubeVideo("Max Play Video");
                try
                {
                    maxPlayVideo.IncreasePlayCount(int.MaxValue); 
                    user.AddVideo(maxPlayVideo);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

             
                Console.WriteLine("\nUji overflow play count:");
                SayaTubeVideo overflowVideo = new SayaTubeVideo("Overflow Test");
                for (int i = 0; i < 100; i++)
                {
                    overflowVideo.IncreasePlayCount(25000000);
                    if (overflowVideo.GetPlayCount() >= int.MaxValue) break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Umum: {ex.Message}");
            }
        }
    }
}