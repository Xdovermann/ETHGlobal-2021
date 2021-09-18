Moralis.initialize("ybhxQ8kz0ffLEhgSTbhn3c0nLQy6OkeUHohFI9nS");
Moralis.serverURL = 'https://afbk8agfcz6r.grandmoralis.com:2053/server';


// login function
async function Login(){

  // returns back the user adress to unity

  let user = Moralis.User.current();
  if(!user){
    
  
    user = await Moralis.authenticate({signingMessage:"Welcome to Chain Crusaders!"});
    const userAddress = user.get('ethAddress');
    UnityInstance.SendMessage("GameManager", "GetUserAdress",userAddress);

  }else{

    const userAddress = user.get('ethAddress');
    UnityInstance.SendMessage("GameManager", "GetUserAdress",userAddress);

  }
}

