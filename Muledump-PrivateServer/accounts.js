var email = prompt("Please enter your email", "");
var password = prompt("Please enter your password", "");
accounts = { }

accounts[email] = password;
console.log(accounts)

// change to 1 to switch to testing
//testing = 0

// change to 1 to enable price display in tooltips
//prices = 1

// 0 = use smart layout (fill empty spaces)
// 1 = show account boxes row by row
//nomasonry = 1