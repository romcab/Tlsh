# Tlsh


C# port from https://github.com/idealista/tlsh

TLSH is a fuzzy matching library designed by Trend Micro (Hosted in GitHub)

Given a byte stream with a minimum length of 512 characters (and a minimum amount of randomness), TLSH generates a hash value which can be used for similarity comparisons. Similar objects will have similar hash values which allows for the detection of similar objects by comparing their hash values. Note that the byte stream should have a sufficient amount of complexity. For example, a byte stream of identical bytes will not generate a hash value.

The computed hash is 70 hexadecimal characters long. The first 6 characters are used to capture the information about the file as a whole (length, ...), while the last 64 characters are used to capture information about incremental parts of the file.

JavaScript port (originally designed for use with Node.js) can be found here.
