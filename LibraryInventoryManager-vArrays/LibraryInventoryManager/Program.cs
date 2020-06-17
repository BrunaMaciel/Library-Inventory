﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryInventoryManager
{
       class Program
    {
        struct Record
        {
            public int type; //1-book, 2-magazine, 3- CD, 4-DVD
            public string title;
            public string artist;
            public double value;
            public string ISBN;
            public int avaliability; //1-in stock, 2-avaliable, 3-on loan
            public string memberID;
        }

        struct Member
        {
            public string firstName;
            public string lastName;
            public string phone;
            public string ID;
            public string recordID;
        }

       static Record[] records;
       static Member[] members;
       static int recQtd=0, memQtd=0;
       static int MAXRecordsAvaliable = 30;
        static int Main(string[] args)
        {
            try
            {
                Console.Write("Please inform how many records will be added: ");
                int recordsMAX = int.Parse(Console.ReadLine());
                records = new Record[recordsMAX];

                Console.Write("Please inform how many members will be added: ");
                int membersMAX = int.Parse(Console.ReadLine());
                members = new Member[membersMAX];

                int opt = displayMenu();
                while (opt != 6)
                {
                    switch (opt) {
                        case 1:
                            addRecord();
                            opt = displayMenu();
                            break;
                        case 2:
                            lendRecord();
                            opt = displayMenu();
                            break;
                        case 3:
                            returnRecord();
                            opt = displayMenu();
                            break;
                        case 4:
                            addMember();
                            opt = displayMenu();
                            break;
                        case 5:
                            listOnLoanRecords();
                            opt = displayMenu();
                            break;
                        case 6:
                            return (0);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please select one of the menu options.");
                            opt = displayMenu();
                            break;
                    }
                }
            }
            catch (FormatException fEx)
            {
                Console.WriteLine(fEx.Message);
            }
            catch (IndexOutOfRangeException iEx)
            {
                Console.WriteLine(iEx.Message);
            }
            return (1);
        }

        static int displayMenu()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Library Inventory Managment - Main Menu");
            Console.WriteLine("1 - Add Record");
            Console.WriteLine("2 - Lend Record");
            Console.WriteLine("3 - Return Record");
            Console.WriteLine("4 - Add Member");
            Console.WriteLine("5 - List records on loan");
            Console.WriteLine("6 - Quit program");
            Console.Write("Please select your option number: ");
            int opt = int.Parse(Console.ReadLine());

            return opt;
        }

        static void addRecord()
        {
            Regex rxISBN = new Regex(@"^\d{13}$");
            int tries = 2;

            Console.WriteLine("\n");
            Console.WriteLine("Library Inventory Managment - Add Record");
            Console.Write("Enter the record type number");
            Console.Write("(1-book, 2-magazine, 3- CD, 4-DVD): ");
            int type = int.Parse(Console.ReadLine());

            Console.Write("Enter the record artist: ");
            string artist = Console.ReadLine();

            Console.Write("Enter the record title: ");
            string title = Console.ReadLine();

            Console.Write("Enter the record ISBN: ");
            string isbn = Console.ReadLine();
            while(!rxISBN.IsMatch(isbn) && tries > 0){
                Console.WriteLine("Invalid ISBN.Please try again, you have {0} more chances.", tries);
                tries--;
                Console.Write("Enter the record ISBN: ");
                isbn = Console.ReadLine();
            }
            if (tries == 0)
            {
                Console.WriteLine("\nInvalid ISBN. You reached the maximum atempts.");
                return;
            }
                
            Console.Write("Enter the record value: ");
            double value = double.Parse(Console.ReadLine());

            Console.WriteLine("The record is avaliable to loan?");
            Console.Write("1-No (in stock) \n2-Yes \nOption: ");
            int avaliability = int.Parse(Console.ReadLine());
            if (avaliability == 2)
                if (MAXRecordsAvaliable > 0)
                {
                    MAXRecordsAvaliable--;
                    records[recQtd].avaliability = avaliability;
                }
                else
                {
                    Console.WriteLine("There are already 30 records avaliable for loan. These record is now in stock.");
                    records[recQtd].avaliability = 1;
                }
            else
                records[recQtd].avaliability = avaliability;
           
            records[recQtd].artist = artist;
            records[recQtd].title = title;
            records[recQtd].ISBN = isbn;
            records[recQtd].value = value;
            records[recQtd].type = type;
            records[recQtd].memberID = "";
            recQtd++;
            
        }
        static void addMember()
        {
            Regex rxPhone = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
            Regex rxID = new Regex(@"^\d{5}$");
            int tries = 2;

            Console.WriteLine("Library Inventory Managment - Add Member");
            Console.Write("Member First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Member Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter the member phone numer: ");
            string phone = Console.ReadLine();
            while (!rxPhone.IsMatch(phone) && tries > 0)
            {
                Console.WriteLine("Invalid phone number.Please try again, you have {0} more chances.", tries);
                tries--;
                Console.Write("Enter the member phone numer: ");
                phone = Console.ReadLine();

            }
            if (tries == 0)
            {
                Console.WriteLine("\nInvalid phone number. You reached the maximum atempts.");
                return;
            }
            tries = 2;

            Console.Write("Enter the member ID (must be 5 digits): ");
            string memberID = Console.ReadLine();
            while (!rxID.IsMatch(memberID) && tries > 0)
            {
                Console.WriteLine("Invalid memberID.Please try again, you have {0} more chances.", tries);
                tries--;
                Console.Write("Enter the member ID (must be 5 digits): ");
                phone = Console.ReadLine();
            }
            if (tries == 0)
            {
                Console.WriteLine("\nInvalid memberID. You reached the maximum atempts.");
                return;
            }
            members[memQtd].firstName = firstName;
            members[memQtd].lastName = lastName;
            members[memQtd].phone = phone;
            members[memQtd].ID = memberID;
            members[memQtd].recordID = "";
            memQtd++;
        }

        static void lendRecord()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Library Inventory Managment - Lend Record");
            Console.Write("Enter the record ID: ");
            string record = Console.ReadLine();

            int i = findRecord(record);
            if (i != -1)
            {
                if (records[i].avaliability == 1)
                {
                    Console.WriteLine("Record is on inventory and not for loan.");
                }
                if (records[i].avaliability == 3)
                {
                    Console.WriteLine("Record already on loan.");
                }
                if (records[i].avaliability == 2)
                {
                    Console.Write("Enter the user ID: ");
                    string member = Console.ReadLine();
                    int j = findMember(member);
                    if (j != -1)
                    {
                        if (members[j].recordID == "")
                        {
                            records[i].avaliability = 3;
                            records[i].memberID = member;
                            members[j].recordID = record;
                            Console.WriteLine("Record {0} lended to Member {1}", records[i].ISBN, members[j].ID);
                        }
                        else
                        {
                            Console.WriteLine("The member {0} already has the record {1} on loan. Please return the record before lending another.", members[j].ID, members[j].recordID);
                        }
                        
                    }
                }

            }
        }

        static int findRecord(string recordID)
        {
            for (int cnt = 0; cnt<recQtd; cnt++)
            {
                if (records[cnt].ISBN.Equals(recordID))
                {
                    return cnt;
                }
            }
            Console.WriteLine("Record does not exist.");
            return -1;
        }

        static int findMember(string memberID)
        {
            for (int cnt = 0; cnt < recQtd; cnt++)
            {
                if (members[cnt].ID.Equals(memberID))
                {
                    return cnt;
                }
            }
            Console.Write("Member does not exist.");
            return -1;
        }

        static void returnRecord()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Library Inventory Managment - Return Record");
            Console.Write("Enter the record ID: ");
            string record = Console.ReadLine();

            int i = findRecord(record);
            if (i != -1)
            {
                if (records[i].avaliability == 1)
                {
                    Console.WriteLine("\nRecord is on inventory and can`t be returned");
                }
                if (records[i].avaliability == 2)
                {
                    Console.WriteLine("\nRecord already avaliable for loan.");
                }
                if (records[i].avaliability == 3)
                {
                    records[i].avaliability = 2;
                    int j = findMember(records[i].memberID);
                    members[j].recordID = "";
                    records[i].memberID = "";
                    Console.WriteLine("\nRecord {0} avaliable for loan", records[i].ISBN);
                }

            }

        }

        static void listOnLoanRecords()
        {
            int cnt = 0;
            foreach(Record r in records)
            {
                if (r.avaliability == 3)
                {
                    cnt++;
                    Console.WriteLine("Record {0} lended to Member {1}", r.ISBN, r.memberID);
                }
            }
            if (cnt == 0)
            {
                Console.WriteLine("\n The are no records on loan at the moment.");
            }

        }
    }
}