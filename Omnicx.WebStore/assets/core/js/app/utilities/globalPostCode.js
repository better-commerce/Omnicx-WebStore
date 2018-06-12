var regxGlobalPostCodes= [
   {
     "Note": "Known as the postcode. The first letter(s) indicate the postal area, such as the town or part of London. Placed on a separate line below the city (or county, if used). The UK postcode is made up of two parts separated by a space. These are known as the outward postcode and the inward postcode. The outward postcode is always one of the following formats: AN, ANN, AAN, AANN, ANA, AANA, AAA. The inward postcode is always formatted as NAA. A valid inward postcode never contains the letters: C, I, K, M, O or V. The British Forces Post Office has a different system, but as of 2012 has also adopted UK-style postcodes that begin with \"BF1\" for electronic compatibility.",
     "Country": "United Kingdom",
     "ISO": "GB",
     "Format": "A(A)N(A/N)NAA (A[A]N[A/N] NAA)",
     "Regex": "[A-Z]{1,2}[0-9R][0-9A-Z]? (?:(?![CIKMOV])[0-9][a-zA-Z]{2})"
   }
]