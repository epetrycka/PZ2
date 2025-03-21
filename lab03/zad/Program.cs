﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using System.Globalization;
using System.Text.Json.Serialization;
public class Tweet
{
    public string Text { get; set; }
    public string UserName { get; set; }
    public string LinkToTweet { get; set; }
    public string FirstLinkUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public string TweetEmbedCode { get; set; }
}

class Program
{
    static void Main()
    {
        string filePath = "favorite-tweets.jsonl";
        List<Tweet> tweets = ReadTweetsFromJsonL(filePath);
        
        Console.WriteLine($"Loaded {tweets.Count} tweets.");
        
        ConvertTweetsToXml(tweets, "tweets.xml");
        List<Tweet> loadedFromXml = LoadTweetsFromXml("tweets.xml");
        
        var sortedByUser = tweets.OrderBy(t => t.UserName).ToList();
        var sortedByDate = tweets.OrderBy(t => t.CreatedAt).ToList();
        
        Console.WriteLine($"Oldest Tweet: {sortedByDate.First().Text}");
        Console.WriteLine($"Newest Tweet: {sortedByDate.Last().Text}");

        var sortedTweets = SortTweetsByUserName(tweets);
        var sortedUsers = SortUsersByFirstTweetDate(tweets);

        Console.WriteLine("Top 5 users sorted by first tweet date:");
        foreach (var user in sortedUsers.Take(5))
        {
            Console.WriteLine(user);
        }
        
        var tweetsByUser = tweets.GroupBy(t => t.UserName).ToDictionary(g => g.Key, g => g.ToList());
        
        var wordFrequencies = CalculateWordFrequencies(tweets);
        var topWords = wordFrequencies.Where(w => w.Key.Length >= 5).OrderByDescending(w => w.Value).Take(10);
        
        Console.WriteLine("Top 10 most frequent words with at least 5 letters:");
        foreach (var word in topWords)
        {
            Console.WriteLine($"{word.Key}: {word.Value}");
        }
        var idfScores = CalculateIDF(tweets);
        var topIDFWords = idfScores.OrderByDescending(kvp => kvp.Value).Take(10);

        Console.WriteLine("Top 10 words with highest IDF:");
        foreach (var word in topIDFWords)
        {
            Console.WriteLine($"{word.Key}: {word.Value:F4}");
        }
    }

    static List<Tweet> ReadTweetsFromJsonL(string filePath)
    {
        List<Tweet> tweets = new List<Tweet>();

        foreach (var line in File.ReadLines(filePath))
        {
            var jsonDoc = JsonDocument.Parse(line);
            var root = jsonDoc.RootElement;

            var tweet = new Tweet
            {
                Text = root.GetProperty("Text").GetString(),
                UserName = root.GetProperty("UserName").GetString(),
                LinkToTweet = root.GetProperty("LinkToTweet").GetString(),
                FirstLinkUrl = root.GetProperty("FirstLinkUrl").GetString(),
                TweetEmbedCode = root.GetProperty("TweetEmbedCode").GetString(),
                CreatedAt = DateTime.ParseExact(
                            root.GetProperty("CreatedAt").GetString(),
                            "MMMM dd, yyyy 'at' hh:mmtt",
                            CultureInfo.InvariantCulture)
            };
            tweets.Add(tweet);
        }
        return tweets;
    }

    static void ConvertTweetsToXml(List<Tweet> tweets, string filePath)
    {
        var xElement = new XElement("Tweets",
            tweets.Select(t => new XElement("Tweet",
                new XElement("Text", t.Text),
                new XElement("UserName", t.UserName),
                new XElement("LinkToTweet", t.LinkToTweet),
                new XElement("CreatedAt", t.CreatedAt))));
        xElement.Save(filePath);
    }

    static List<Tweet> LoadTweetsFromXml(string filePath)
    {
        XElement xElement = XElement.Load(filePath);
        return xElement.Elements("Tweet").Select(t => new Tweet
        {
            Text = t.Element("Text")?.Value,
            UserName = t.Element("UserName")?.Value,
            LinkToTweet = t.Element("LinkToTweet")?.Value,
            CreatedAt = DateTime.Parse(t.Element("CreatedAt")?.Value)
        }).ToList();
    }

    static List<Tweet> SortTweetsByUserName(List<Tweet> tweets)
    {
        return tweets.OrderBy(t => t.UserName).ToList();
    }

    static List<string> SortUsersByFirstTweetDate(List<Tweet> tweets)
    {
        return tweets.GroupBy(t => t.UserName)
                    .Select(g => new { UserName = g.Key, FirstTweetDate = g.Min(t => t.CreatedAt) })
                    .OrderBy(u => u.FirstTweetDate)
                    .Select(u => u.UserName)
                    .ToList();
    }

    static Dictionary<string, int> CalculateWordFrequencies(List<Tweet> tweets)
    {
        Dictionary<string, int> wordCounts = new Dictionary<string, int>();
        foreach (var tweet in tweets)
        {
            string[] words = tweet.Text.Split(new[] { ' ', '.', ',', '!', '?', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                string lowerWord = word.ToLower();
                if (wordCounts.ContainsKey(lowerWord))
                    wordCounts[lowerWord]++;
                else
                    wordCounts[lowerWord] = 1;
            }
        }
        return wordCounts;
    }

    static Dictionary<string, double> CalculateIDF(List<Tweet> tweets)
    {
        int totalTweets = tweets.Count;
        Dictionary<string, int> documentFrequency = new Dictionary<string, int>();

        foreach (var tweet in tweets)
        {
            HashSet<string> uniqueWords = new HashSet<string>(
                tweet.Text.Split(new[] { ' ', '.', ',', '!', '?', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(word => word.ToLower()));

            foreach (var word in uniqueWords)
            {
                if (documentFrequency.ContainsKey(word))
                    documentFrequency[word]++;
                else
                    documentFrequency[word] = 1;
            }
        }

        Dictionary<string, double> idfScores = new Dictionary<string, double>();
        foreach (var kvp in documentFrequency)
        {
            idfScores[kvp.Key] = Math.Log((double)totalTweets / kvp.Value);
        }

        return idfScores;
    }
}