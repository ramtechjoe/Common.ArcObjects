# Ram.Common.ArcObjects

The Common ArcObject library was developed to take common ArcObject operations and have them return .net IEnumerble objects.  
These are implemented as extension methods on standard ArcObject interfaces.

Other functions are added which normally require mutiple casts to find the generally desired interface.


A comparer is also included to compare to features based on having the same class name and ObjectID.

## Version
The library was recently updated to use ArcObject 10.8.1.  In order to maintain compatibility with ArcFM .net framework 10.6.1 is used

### Samples

```cs
IQueryFilter filter = new QueryFilterClass();
var features = featureClass.Search(filter);

for (feature in features)
{
	// perform operation on feature
}

```

