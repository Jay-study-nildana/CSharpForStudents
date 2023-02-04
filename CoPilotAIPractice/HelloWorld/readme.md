# Hello world with GitHub AI

my first c sharp project where I simply let the AI go wild without any purpose.

# some observations

1. The AI simply starts showing me the basics of c sharp as i keep tabbing and entering. I know by now that it will simply begin acting like an 'auto tutor', teaching me general things, and eventually running out of steam. 
1. It does not seem to remember what it did before. for example, it did this.
    ```
    //for loop

    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);
    }
    ```
    and then, did this, leading to errors due to redeclaraion of functions and variables.
    ```
    //while loop

    int i = 0;

    while (i < 10)
    {
        Console.WriteLine(i);
        i++;
    }
    ```
1. the redeclaration continues to functions as well.
    ```
    public class Program
    {
        public static void Main()
        {
            MyDelegate del = new MyDelegate(Method1);
            del();
        }

        public static void Method1()
        {
            Console.WriteLine("Method1");
        }
    }
    ```
    and like this
    ```
    public class Program
    {
        public static void Main()
        {
            MyEvent evt = new MyEvent();
            evt.MyEventName += new MyEventHandler(Method1);
            evt.OnMyEvent();
        }

        public static void Method1()
        {
            Console.WriteLine("Method1");
        }
    }    
    ```
1. Ultimately, there is still a need for the developer to know what he is doing, to use the AI effectively.

# Hire Me

I work as a full time freelance coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# Hobbies

I try to maintain a few hobbies.

1. Podcasting. You can listen to my daily life [podcast](https://stories.thechalakas.com/listen-to-podcast/).
1. Podcasting. You can listen to my movies [podcast](https://sandkdesignstudio.in/jays-movie-podcast/).
1. Photography Nature. You can see my photography on [Unsplash](https://unsplash.com/@jay_neeruhaaku).
1. Photography Fashion. You can see my fashion photography on [Behance](https://www.behance.net/vijayasimhabr)
1. Digital Photorealism 3D Art. You can see my work on [ArtStation](https://www.artstation.com/jay_kalenildana).
1. Daily Life Blog. [Read it here](https://medium.com/the-sanguine-tech-trainer).
1. Coding and Technology Blog. [Read it here](https://medium.com/projectwt).
1.  Daz 3D, Photography and Photoshop Blog. [Read it here](https://medium.com/random-pink-hula).

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

1. Jay's [Developer Profile](https://jay-study-nildana.github.io/developerprofile)
1. Jay's [Personal Site](https://stories.thechalakas.com/)