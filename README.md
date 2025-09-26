# moovup-test

Language:  
- C# .Net 8

  
Instruction:  
- Execute `dotnet run` given .net runtime installed in the environment.  
- Test code will be run and results will be displayed in console.

Explanation:  
- Use Dictionary for specific user bucket retrieval in O(1) time performance.
- Bucket updates takes O(1) time if below capacity; otherwise O(n) linear with capacity.
- Bucket of a user would only be evaluated and cleaned up for leakage until next request of this user. No schedule clean up is implemented. This implies excessive storage cost for obsolete bucket data.
- Backdate request will also be rejected once bucket is full. This still aligns with the purpose of rate limit.
- No sync lock on limiter status implemented to handle conccurent requests which would be required in real situation.
