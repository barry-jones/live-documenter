# Signitures Excersise Tests

These tests are to excersise all of the signiture types in the Reflection.Signiture 
namespace. Signitures are loaded from the Metadata heaps and then resolved to types,
methods, parameters and generic types inside of the application.

We need to test the signiture parsing code against a ton of different libraries to
make sure there are no edge cases we are missing and that stuff is being processed
correctly (and later resolved).