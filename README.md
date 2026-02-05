# Xunit Extensions

## What this Repo is for?

This repo is for potentially useful extensions to the xunit test framework. It is published as the `Audacia.XunitExtensions` NuGet package to the Audacia private NuGet feed.

## Retrying Brittle Tests

In certain circumstances (for example UI automation) it can be necessary to retry some tests on failure as the failure may be intermittent.

The `[BrittleFactAttribute]` and `[BrittleTheoryAttribute]` allow a test to be retried up to a configurable number of times. As soon as the test passes then it returns a pass result as normal. However if the test fails then it continues trying until it reaches the given number of retries.

For example the below test would retry on failure up to 3 times (so a maximum of 4 execution attempts):
```csharp
[BrittleFact(3)]
public void A_Brittle_Test()
{
    // test code here
}
```

## Inconclusive Test Results

It can sometimes be useful to attempt to run a test, but then mark the result as 'inconclusive' (rather than pass or fail). This functionality is available in both MSTest and NUnit via the `Assert.Inconclusive` method. Similar functionality can be simulated by extending xunit to include a `[InconclusiveFactAttribute]` and `[InconclusiveTheoryAttribute]`. Throwing a particular exception from within the test (`InconclusiveTestResultException`) will result in an 'inconclusive' result - the same result as if a test is skipped or ignored.

For example:
```csharp
[InconclusiveFact]
public void Potentially_Inconclusive_Test()
{
    // some test code

    if (inconclusiveCondition)
    {
        throw new InconclusiveTestResultException("reason");
    }

    // more test code
}
```

# Change History
The `Audacia.XunitExtensions` repository change history can be found in this [changelog](./CHANGELOG.md):

# Contributing
We welcome contributions! Please feel free to check our [Contribution Guidlines](https://github.com/audaciaconsulting/.github/blob/main/CONTRIBUTING.md) for feature requests, issue reporting and guidelines.
