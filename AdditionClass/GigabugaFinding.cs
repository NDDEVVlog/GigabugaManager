using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiGaBuGaManager.AdditionClass
{
    public class GigabugaFinding
    {
        private static GigabugaFinding instance = null;
        private static readonly object padlock = new object();

        public Dictionary<string, HashSet<string>> songsTags;

        private GigabugaFinding()
        {
            songsTags = new Dictionary<string, HashSet<string>>();
        }

        public static GigabugaFinding Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GigabugaFinding();
                    }
                    return instance;
                }
            }
        }

        public void AddSong(string song, string tags)
        {
            /*if (string.IsNullOrEmpty(song) || string.IsNullOrEmpty(tags))
            {
                throw new ArgumentException("Song and tags must not be empty.");
            }*/
            if (tags == null) return;

            var tagSet = new HashSet<string>(tags.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(tag => tag.Trim()));

            if (!songsTags.ContainsKey(song))
            {
                songsTags[song] = tagSet;
            }
            else
            {
                songsTags[song].UnionWith(tagSet);
            }
        }

        public List<string> FindSongs(string tags)
        {
            var inputTags = new HashSet<string>(tags.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(tag => tag.Trim()));
            var matchingFiles = new List<string>();

            foreach (var song in songsTags)
            {
                if (inputTags.All(tag => song.Value.Contains(tag)))
                {
                    matchingFiles.Add(song.Key);
                }
            }

            return matchingFiles;
        }
        public void DisplaySongs()
        {
            foreach (var song in songsTags)
            {
                Console.WriteLine($"{song.Key}: {string.Join(", ", song.Value)}");
            }
        }
    }
}
