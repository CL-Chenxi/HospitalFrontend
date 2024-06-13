
//PrintDelegate PrintConsole = (string text) =>  //PrintDelegate is variable
//{
//    Console.WriteLine(text);
//};

//PrintDelegate PrintToFile = (string text) =>  //PrintDelegate is variable
//{
//    File.AppendAllText("./logs.txt", text);
//};

//static void connectToDatabase(PrintDelegate log)  //PrinteDelegate here is a parameter
//{
//    log("Inserting a new record into DB");
//    log("The record got inserted into DB");
//}

//connectToDatabase(PrintToFile);


//delegate void PrintDelegate(string text); //created delegate



////https://www.youtube.com/watch?v=uAhVpw8zzm4  prmote functional programing, passing a function into other functions