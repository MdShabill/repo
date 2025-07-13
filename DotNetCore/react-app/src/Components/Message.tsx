//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915732
//Keeping Components Pure

// const Message = () => {
//   let count = 0;
//   count++;
//   return <div>Message {count}</div>;
// };

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915727
//Understanding Strict Mode

let count = 0;

const Message = () => {
  console.log("Message called", count);
  count++;
  return <div>Message {count}</div>;
};

export default Message;

// function Message(){
//     const myName = 'Shabill Irfani';
//     if(myName)
//     return <h1>Hello {myName}</h1>;
//     return <h1>Hello world...</h1>
// }

// export default Message;
