using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FlickrResearcherLibrary
{
    /// <summary>
    /// Enum of type user mood
    /// </summary>
    public enum TypeMoodUser
    {
        Good,
        Bad,
        Cool
    }

    public class User
    {
        public static User CurrentUser { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public TypeMoodUser TypeMood { get; set; }

        public User() { }
        public User(string name, string city, TypeMoodUser type)
        {
            Name = name;
            City = city;
            TypeMood = type;
        }

        /// <summary>
        /// Return type mood
        /// </summary>
        /// <param name="mood"></param>
        /// <returns></returns>
        public TypeMoodUser ChooseMood(string mood)
        {
            TypeMoodUser type;
            try
            {
                if (Enum.TryParse(mood, out type))
                    switch (mood)
                    {
                        case "Good":
                            return TypeMoodUser.Good;
                        case "Bad":
                            return TypeMoodUser.Bad;
                        case "Cool":
                            return TypeMoodUser.Cool;
                        default:
                            break;
                    }
                return TypeMoodUser.Cool;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Load moods image
        /// </summary>
        /// <param name="type"></param>
        public BitmapImage LoadMood(TypeMoodUser type)
        {
            switch (type)
            {
                case TypeMoodUser.Bad:
                    return new BitmapImage(new Uri("pack://application:,,,/media/bad_mood.png"));
                case TypeMoodUser.Cool:
                    return new BitmapImage(new Uri("pack://application:,,,/media/cool_mood.png"));
                case TypeMoodUser.Good:
                    return new BitmapImage(new Uri("pack://application:,,,/media/good_mood.png"));
                default:
                    break;
            }
            return null;
        }
    }
}
