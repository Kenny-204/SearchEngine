using System;
using System.Collections.Generic;
using System.Linq;
using SearchEngine.Query.Core;

namespace SearchEngine.Query.Algorithms
{
    /// <summary>
    /// Implementation of the Porter Stemming Algorithm
    /// </summary>
    public class PorterStemmer : IPorterStemmer
    {
        private readonly HashSet<char> _vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };

        public string Stem(string word)
        {
            if (string.IsNullOrEmpty(word) || word.Length < 3)
                return word;

            var result = word.ToLowerInvariant();
            
            // Apply the Porter Stemmer steps
            result = Step1(result);
            result = Step2(result);
            result = Step3(result);
            result = Step4(result);
            result = Step5(result);
            
            return result;
        }

        private string Step1(string word)
        {
            // Step 1a: Remove plurals
            if (word.EndsWith("sses"))
                return word.Substring(0, word.Length - 2);
            if (word.EndsWith("ies"))
                return word.Substring(0, word.Length - 2);
            if (word.EndsWith("ss"))
                return word;
            if (word.EndsWith("s"))
                return word.Substring(0, word.Length - 1);

            // Step 1b: Remove past participles and other endings
            if (word.EndsWith("eed"))
            {
                if (Measure(word.Substring(0, word.Length - 3)) > 0)
                    return word.Substring(0, word.Length - 1);
                return word;
            }

            if (word.EndsWith("ed"))
            {
                var stem = word.Substring(0, word.Length - 2);
                if (ContainsVowel(stem))
                    return Step1b2(stem);
                return word;
            }

            if (word.EndsWith("ing"))
            {
                var stem = word.Substring(0, word.Length - 3);
                if (ContainsVowel(stem))
                    return Step1b2(stem);
                return word;
            }

            // Handle er and est endings in Step 1b
            if (word.EndsWith("er"))
            {
                var stem = word.Substring(0, word.Length - 2);
                if (ContainsVowel(stem) && Measure(stem) > 0)
                    return stem;
                return word;
            }

            if (word.EndsWith("est"))
            {
                var stem = word.Substring(0, word.Length - 3);
                if (ContainsVowel(stem) && Measure(stem) > 0)
                    return stem;
                return word;
            }

            return word;
        }

        private string Step1b2(string stem)
        {
            if (stem.EndsWith("at"))
                return stem + "e";
            if (stem.EndsWith("bl"))
                return stem + "e";
            if (stem.EndsWith("iz"))
                return stem + "e";

            if (EndsWithDoubleConsonant(stem) && !EndsWithCVC(stem))
                return stem.Substring(0, stem.Length - 1);

            if (Measure(stem) == 1 && EndsWithCVC(stem))
                return stem + "e";

            return stem;
        }

        private string Step2(string word)
        {
            var replacements = new Dictionary<string, string>
            {
                { "ational", "ate" },
                { "tional", "tion" },
                { "enci", "ence" },
                { "anci", "ance" },
                { "izer", "ize" },
                { "abli", "able" },
                { "alli", "al" },
                { "entli", "ent" },
                { "eli", "e" },
                { "ousli", "ous" },
                { "ization", "ize" },
                { "ation", "ate" },
                { "ator", "ate" },
                { "alism", "al" },
                { "iveness", "ive" },
                { "fulness", "ful" },
                { "ousness", "ous" },
                { "aliti", "al" },
                { "iviti", "ive" },
                { "biliti", "ble" },
                { "ly", "" }  // Handle ly endings in Step 2
            };

            foreach (var replacement in replacements)
            {
                if (word.EndsWith(replacement.Key))
                {
                    var stem = word.Substring(0, word.Length - replacement.Key.Length);
                    if (Measure(stem) > 0)
                        return stem + replacement.Value;
                }
            }

            return word;
        }

        private string Step3(string word)
        {
            var replacements = new Dictionary<string, string>
            {
                { "icate", "ic" },
                { "ative", "" },
                { "alize", "al" },
                { "iciti", "ic" },
                { "ical", "ic" },
                { "ful", "" },
                { "ness", "" }
            };

            foreach (var replacement in replacements)
            {
                if (word.EndsWith(replacement.Key))
                {
                    var stem = word.Substring(0, word.Length - replacement.Key.Length);
                    if (Measure(stem) > 0)
                        return stem + replacement.Value;
                }
            }

            return word;
        }

        private string Step4(string word)
        {
            var suffixes = new[] { "al", "ance", "ence", "ic", "able", "ible", "ant", "ement", "ment", "ent", "ion", "ou", "ism", "ate", "iti", "ous", "ive", "ize" };

            foreach (var suffix in suffixes)
            {
                if (word.EndsWith(suffix))
                {
                    var stem = word.Substring(0, word.Length - suffix.Length);
                    if (Measure(stem) > 1)
                        return stem;
                }
            }

            return word;
        }

        private string Step5(string word)
        {
            // Step 5a: Remove final 'e' if measure > 1
            if (word.EndsWith("e"))
            {
                var stem = word.Substring(0, word.Length - 1);
                if (Measure(stem) > 1)
                    return stem;
                if (Measure(stem) == 1 && !EndsWithCVC(stem))
                    return stem;
            }

            // Step 5b: Remove final consonant if measure > 1 and ends with double consonant
            if (EndsWithDoubleConsonant(word) && Measure(word) > 1)
                return word.Substring(0, word.Length - 1);

            return word;
        }

        private bool ContainsVowel(string word)
        {
            return word.Any(c => _vowels.Contains(c));
        }

        private bool EndsWithDoubleConsonant(string word)
        {
            if (word.Length < 2)
                return false;

            var last = word[word.Length - 1];
            var secondLast = word[word.Length - 2];

            return last == secondLast && !_vowels.Contains(last);
        }

        private bool EndsWithCVC(string word)
        {
            if (word.Length < 3)
                return false;

            var last = word[word.Length - 1];
            var secondLast = word[word.Length - 2];
            var thirdLast = word[word.Length - 3];

            return !_vowels.Contains(last) && _vowels.Contains(secondLast) && !_vowels.Contains(thirdLast);
        }

        private int Measure(string word)
        {
            var measure = 0;
            var previousWasVowel = false;

            for (int i = 0; i < word.Length; i++)
            {
                var isVowel = _vowels.Contains(word[i]);
                
                if (isVowel && !previousWasVowel)
                    measure++;
                
                previousWasVowel = isVowel;
            }

            return measure;
        }
    }
} 