**********************************************************************
*                                                                    *
* ReadASCIIdata.sps - Read ASCII data and save as an SPSS data file. *
*                                                                    *
**********************************************************************
*                                                                    *
* To run these syntax commands select All from the Run menu in the   *
* SPSS Syntax Editor.                                                *
*                                                                    *
**********************************************************************
*                                                                    *
* Personnel.dat - ASCII data file                                    *
* Personnel.sav - SPSS data file created                             *
*                                                                    *
* The purpose of this example is to illustrate:                      *
*   1.  Data input from the data file Personnel.dat via the DATA     *
*       LIST command.                                                *
*   2.  Saving data & a data dictionary (includes all labels and     *
*       missing value codes) as an SPSS data file via the SAVE       *
*       OUTFILE command. Note that you can also create an SPSS data  *
*       file by selecting Save from the File menu in the SPSS Data   *
*       Editor.                                                      *
*                                                                    *
* Note the location of both data files is the StatsExamples folder   *
* in the H: drive.                                                   *
*                                                                    *
**********************************************************************.

data list file='h:\StatsExamples\Personnel.dat' fixed records=2 
  /1 name 1-24(A) employid 26-30
  /2 yrhired 3-4 age 6-7 race 9-9 sex 11-11 locatn82 13-13 dept82 15-15 
     jobcat 17-17 promo82 19-19 salary82 21-25 raise82 27-31 
     eeo82 33-33.

variable labels
   name     "Employee's Name"
   employid "Employee's Badge Number"
   yrhired  "Year of First Hiring"
   age      "Employee's Age in 1980"
   race     "Employee's Race"
   sex      "Employee's Sex"
   locatn82 "City Where Employed"
   dept82   "Department Code in 1982"
   jobcat   "Job Category"
   promo82  "Was Emp Promoted in 1982?"
   salary82 "Yearly Salary in 1982"
   raise82  "Increase in Salary over 1981".

value labels
   race 1 'Black' 2 'A.Indian' 3 'Oriental' 4 'Latino'
        5 'White'
  /sex 1 'Male' 2 'Female'
  /locatn82 0 'Not Employed' 1 'Chicago' 5 'St. Louis'
  /dept82 0 'Not Employed' 1 'Administrative' 2 'Project Directors'
          3 'Chicago Operations' 4 'St. Louis Operations'
  /jobcat 1 'Officials & Managers' 2 'Professionals'
          3 'Technicians' 4 'Office and Clerical' 5 'Craftsmen'
          8 'Service Workers'
  /promo82 0 'No' 1 'Yes' 9 'Not Employed'.

missing values
   yrhired to dept82 salary82 (0)/ promo82 (9)/raise82 (-999).

execute.

save outfile='h:\StatsExamples\Personnel.sav'
   /compressed.
