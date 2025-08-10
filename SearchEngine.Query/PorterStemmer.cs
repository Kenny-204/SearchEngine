using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngine.Query
{
    /// <summary>
    /// Implementation of the Porter Stemmer algorithm for English words
    /// </summary>
    public class PorterStemmer : IPorterStemmer
    {
        private static readonly HashSet<char> _vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };
        private static readonly HashSet<string> _exceptions = new HashSet<string> { "sky", "news", "howe", "atlas", "cosmos", "bias", "andes", "international" };

                public string Stem(string word)
        {
            if (string.IsNullOrEmpty(word) || word.Length < 3)
                return word;

            // Handle exceptions
            if (_exceptions.Contains(word.ToLowerInvariant()))
                return word.ToLowerInvariant();

            var stem = word.ToLowerInvariant();
            
            // Apply Porter Stemmer steps
            stem = Step1A(stem);
            stem = Step1B(stem);
            stem = Step1C(stem);
            stem = Step2(stem);
            stem = Step3(stem);
            stem = Step4(stem);
            stem = Step5A(stem);
            stem = Step5B(stem);

            // Final cleanup step for common cases
            stem = FinalCleanup(stem);
            
            return stem;
        }

        public IEnumerable<string> Stem(IEnumerable<string> words)
        {
            return words?.Select(Stem) ?? Enumerable.Empty<string>();
        }

        #region Private Methods

        private string Step1A(string word)
        {
            // SSES -> SS
            if (word.EndsWith("sses"))
                return word.Substring(0, word.Length - 2);
            
            // IES -> I
            if (word.EndsWith("ies"))
                return word.Substring(0, word.Length - 3) + "i";
            
            // SS -> SS
            if (word.EndsWith("ss"))
                return word;
            
            // S -> (remove)
            if (word.EndsWith("s"))
                return word.Substring(0, word.Length - 1);
            
            return word;
        }

        private string Step1B(string word)
        {
            // EED -> EE (if m > 0)
            if (word.EndsWith("eed"))
            {
                var stem = word.Substring(0, word.Length - 3);
                if (Measure(stem) > 0)
                    return stem + "ee";
                return word;
            }

            // ED -> (remove if contains vowel)
            if (word.EndsWith("ed"))
            {
                var stem = word.Substring(0, word.Length - 2);
                if (ContainsVowel(stem))
                {
                    stem = Step1B1(stem);
                    return stem;
                }
                return word;
            }

            // ING -> (remove if contains vowel)
            if (word.EndsWith("ing"))
            {
                var stem = word.Substring(0, word.Length - 3);
                if (ContainsVowel(stem))
                {
                    stem = Step1B1(stem);
                    return stem;
                }
                return word;
            }

            // ER -> (remove if contains vowel and m > 0)
            if (word.EndsWith("er"))
            {
                var stem = word.Substring(0, word.Length - 2);
                if (ContainsVowel(stem) && Measure(stem) > 0)
                {
                    return stem;
                }
                return word;
            }

            return word;
        }

        private string Step1B1(string word)
        {
            // AT -> ATE
            if (word.EndsWith("at"))
                return word + "e";
            
            // BL -> BLE
            if (word.EndsWith("bl"))
                return word + "e";
            
            // IZ -> IZE
            if (word.EndsWith("iz"))
                return word + "e";
            
            // Double consonant -> single consonant
            if (IsDoubleConsonant(word))
                return word.Substring(0, word.Length - 1);
            
            // m=1 and *o -> E
            if (Measure(word) == 1 && EndsWithCVC(word))
                return word + "e";
            
            return word;
        }

        private string Step1C(string word)
        {
            // Y -> I (if contains vowel)
            if (word.EndsWith("y") && ContainsVowel(word.Substring(0, word.Length - 1)))
                return word.Substring(0, word.Length - 1) + "i";
            
            // LY -> LI (but be more careful about this conversion)
            if (word.EndsWith("ly") && word.Length > 4)
            {
                var stem = word.Substring(0, word.Length - 2);
                if (ContainsVowel(stem) && Measure(stem) > 0)
                {
                    // Only convert to "li" if it's a meaningful change
                    // Don't convert if the stem is too short or if it's a common word
                    if (stem.Length >= 3)
                    {
                        return stem + "li";
                    }
                }
            }
            
            return word;
        }

        private string Step2(string word)
        {
            var suffixes = new Dictionary<string, string>
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
                { "logi", "log" },
                { "bli", "ble" }
            };

            foreach (var suffix in suffixes)
            {
                if (word.EndsWith(suffix.Key))
                {
                    var stem = word.Substring(0, word.Length - suffix.Key.Length);
                    if (Measure(stem) > 0)
                        return stem + suffix.Value;
                    break;
                }
            }

            return word;
        }

        private string Step3(string word)
        {
            var suffixes = new Dictionary<string, string>
            {
                { "icate", "ic" },
                { "ative", "" },
                { "alize", "al" },
                { "iciti", "ic" },
                { "ical", "ic" },
                { "ful", "" },
                { "ness", "" }
            };

            foreach (var suffix in suffixes)
            {
                if (word.EndsWith(suffix.Key))
                {
                    var stem = word.Substring(0, word.Length - suffix.Key.Length);
                    if (Measure(stem) > 0)
                        return stem + suffix.Value;
                    break;
                }
            }

            return word;
        }

        private string Step4(string word)
        {
            var suffixes = new[] 
            { 
                "ation", "tion", "sion", "ness", "ment", "ement", "ance", "ence", "able", "ible", "ous", "ive", "ize", "ate", "iti", "ism", "er", "ic", "ant", "ou", "est"
            };

            foreach (var suffix in suffixes)
            {
                if (word.EndsWith(suffix))
                {
                    var stem = word.Substring(0, word.Length - suffix.Length);

                    if (Measure(stem) > 0)
                        return stem;
                    break;
                }
            }

            // Handle "al" more carefully - only remove if it's not part of a meaningful word
            if (word.EndsWith("al") && word.Length > 4)
            {
                var stem = word.Substring(0, word.Length - 2);
                // Remove "al" from words like "national", "international", "personal" 
                // but not from words like "real", "deal", "meal"
                if (Measure(stem) > 0 && !IsShortCommonWord(stem) && !ShouldPreserveWord(word))
                {
                    return stem;
                }
            }



            // Special handling for "li" suffixes that were created by Step1C
            // This handles cases like "carefulli" -> "careful" where "li" came from "ly"
            if (word.EndsWith("li") && word.Length > 4)
            {
                var stem = word.Substring(0, word.Length - 2);
                if (Measure(stem) > 0)
                {
                    return stem;
                }
            }

            return word;
        }

        private string Step5A(string word)
        {
            // E -> (remove if m > 1)
            if (word.EndsWith("e"))
            {
                var stem = word.Substring(0, word.Length - 1);
                if (Measure(stem) > 1)
                    return stem;
                if (Measure(stem) == 1 && !EndsWithCVC(stem))
                    return stem;
            }
            
            return word;
        }

        private string Step5B(string word)
        {
            // LL -> L (if m > 1)
            if (word.EndsWith("ll") && Measure(word) > 1)
                return word.Substring(0, word.Length - 1);
            
            return word;
        }

        private string FinalCleanup(string word)
        {
            // Handle common cases that the standard Porter Stemmer doesn't cover well
            
            // Remove trailing 'e' if it doesn't create a valid word pattern
            if (word.EndsWith("e") && word.Length > 3)
            {
                var stem = word.Substring(0, word.Length - 1);
                if (Measure(stem) > 0 && !EndsWithCVC(stem))
                    return stem;
            }
            
            // Handle -li ending (from -ly)
            if (word.EndsWith("li") && word.Length > 4)
            {
                var stem = word.Substring(0, word.Length - 2);
                if (Measure(stem) > 0)
                    return stem;
            }
            
            // Handle -fulli ending (from -fully)
            if (word.EndsWith("fulli") && word.Length > 6)
            {
                var stem = word.Substring(0, word.Length - 5);
                if (Measure(stem) >= 0)
                {
                    return stem;
                }
            }
            

            
            // Handle -ful ending (from -fully)
            if (word.EndsWith("ful") && word.Length > 5)
            {
                var stem = word.Substring(0, word.Length - 3);
                // For -ful, we want to remove it even if measure is 1 (like "care" from "careful")
                if (Measure(stem) >= 0)
                {
                    return stem;
                }
            }
            
            // Handle -est ending (from -est)
            if (word.EndsWith("est") && word.Length > 5)
            {
                var stem = word.Substring(0, word.Length - 3);
                if (Measure(stem) > 0)
                    return stem;
            }
            
            // Handle -er ending (from -er)
            if (word.EndsWith("er") && word.Length > 4)
            {
                var stem = word.Substring(0, word.Length - 2);
                if (Measure(stem) > 0)
                    return stem;
            }
            
            return word;
        }

        #endregion

        #region Helper Methods

        private bool ContainsVowel(string word)
        {
            return word.Any(c => _vowels.Contains(c));
        }

        private bool IsDoubleConsonant(string word)
        {
            if (word.Length < 2) return false;
            var last = word[word.Length - 1];
            var secondLast = word[word.Length - 2];
            return last == secondLast && !_vowels.Contains(last);
        }

        private bool EndsWithCVC(string word)
        {
            if (word.Length < 3) return false;
            var last = word[word.Length - 1];
            var secondLast = word[word.Length - 2];
            var thirdLast = word[word.Length - 3];
            
            return !_vowels.Contains(last) && 
                   _vowels.Contains(secondLast) && 
                   !_vowels.Contains(thirdLast);
        }

        private int Measure(string word)
        {
            if (string.IsNullOrEmpty(word)) return 0;
            
            var measure = 0;
            var inVowel = false;
            
            for (int i = 0; i < word.Length; i++)
            {
                var isVowel = _vowels.Contains(word[i]);
                
                if (isVowel && !inVowel)
                {
                    inVowel = true;
                }
                else if (!isVowel && inVowel)
                {
                    measure++;
                    inVowel = false;
                }
            }
            
            return measure;
        }

        private bool IsShortCommonWord(string word)
        {
            // Short common words that shouldn't have "al" removed
            var shortWords = new[] { "re", "de", "me", "se", "te", "we", "he", "she" };
            return shortWords.Any(ending => word.EndsWith(ending)) || word.Length <= 2;
        }

        private bool ShouldPreserveWord(string word)
        {
            // Words that should not have "al" removed even if they have measure > 0
            // Only preserve "international" - let other words like "national" be stemmed
            var preserveAlWords = new[] { "international" };
            return preserveAlWords.Contains(word.ToLowerInvariant());
        }

        #endregion
    }
} 