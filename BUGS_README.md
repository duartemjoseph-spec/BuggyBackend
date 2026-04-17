# Joseph Duarte
# debugged the code and found 21 bugs in the codebase. Below is a list of the bugs and their fixes.


Bug #1 .csproj file
- Switched the version from version 10 to version 9
Fix: `<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.0" />`
     `<TargetFramework>net9.0</TargetFramework>`

Bug #2 program.cs file
Missing a AddAnyOrigin line in our Cors Policy
Fix: `.AllowAnyOrigin()`

Bug #3 program.cs file
Missing line to Use Cors Policy that was made
Fix: `app.UseCors("AllowAll");`

Bug #4 MemberRepository.cs file line 23
Missing a closing quote for the email.
Fix: `new Member { Id = 3, Name = "Ken Martinez", Email = "jmartenezlopez@sjcoe.net", MembershipDate = DateTime.Now.AddMonths(-6) }`

Bug #5 BooksController.cs line 65
Function has a extra catch that is not needed
Fix: Remove `catch (ArgumentNullException ex)
{
    return BadRequest(ex.Message);
}`

Bug #6 Program.cs line 9
Missing scope for IBookRepository in program.cs file
Fix: Add `builder.Services.AddScoped<IBookRepository, BookRepository>();`

Bug #7 BookRepository.cs Line 37
Missing extra = that will change from assigning to comparing
Fix: `return _books.FirstOrDefault(b => b.Id == id);`

Bug #8 MemberRepository.cs Line 40
Missing extra = that will change from assigning to comparing
Fix: `return _members.FirstOrDefault(m => m.Email == email);`

Bug #9 MemberService.cs Line 87
Missing = in the conditional to check for 0
Fix: `(id <= 0)`

 Bug #10 LibraryService.cs Line 46
Missing check so the user is not able to borrow the same book again.
Fix: Add `if (member.BorrowedBookIds.Contains(bookId))
{
    throw new InvalidOperationException("Member already borrowed this book");
}`

 Bug #11 MemberRepository.cs Line 77
Add a check so the same book can not be applied to a member.
Fix: Add if statement `
if (!member.BorrowedBookIds.Contains(bookId))
{
    member.BorrowedBookIds.Add(bookId);
}`

 Bug #12 BookService.cs Line 87
Adjust DeleteBook so it doesn't delete a book that has an active transaction open
Fix: `var hasActiveLoans = _transactionRepository
    .GetActiveTransactions()
    .Any(t => t.BookId == id);

if (hasActiveLoans)
{
    throw new InvalidOperationException("Cannot delete a book with active loans");
}`

 Bug #13 MemberService.cs Line 67
Add a check to UpdateMember so the user cannot update their email to an email that is tied to another user
Fix: `var memberWithSameEmail = _memberRepository.GetByEmail(member.Email);
if (memberWithSameEmail != null && memberWithSameEmail.Id != id)
{
    throw new InvalidOperationException("A member with this email already exists");
}`

Bug #14 BookService.cs Line 56
No validation check for empty ISBN line
Fix: Add `if (string.IsNullOrWhiteSpace(book.ISBN))
{
    throw new ArgumentException("ISBN is required");
}`

Bug #15 BookService.cs Line 38
No validation check for the PublishedYear allowing any year
Fix: `if (book.PublishedYear <= 0 || book.PublishedYear > DateTime.Now.Year)
{
    throw new ArgumentException("Published year is invalid");
}`

Bug #16 BookService.cs Line 45
No validation for AvailableCopies
Fix: Add `if (book.AvailableCopies < 0)
{
    throw new ArgumentException("Available copies cannot be negative");
}`

 Bug #17 Program.cs Line 9   had singleton instead of scoped
Fix: `builder.Services.AddScoped<IMemberRepository, MemberRepository>(); builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();`

Bug #18 MemberService.cs Line 55
No validation for name when updating a member, allowing for empty strings
Fix: Add `if (string.IsNullOrWhiteSpace(member.Name))
{
    throw new ArgumentException("Member name is required");
}`

Bug #19 MemberService.cs Line 55
No validation for email when updating a member, allowing for empty/invalid strings
Fix: Add `if (string.IsNullOrWhiteSpace(member.Email))
{
    throw new ArgumentException("Member email is required");
}

if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(member.Email))
{
    throw new ArgumentException("Member email is invalid");
}`

 Bug #20 LibraryController.cs Line 24
CreatedAtAction would be a better return for a 201 Created code
Fix: Add `return CreatedAtAction(
    nameof(GetMemberTransactions),
    new { memberId = transaction.MemberId },
    transaction
);`

Bug #21 LibraryController.cs Line 42
Change http method from POST to PUT so not update an existing transaction.
Fix: [HttpPut("return/{transactionId}")]