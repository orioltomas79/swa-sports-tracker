import React, { useState, useEffect } from "react";

function App() {
  const [data, setData] = useState("");

  useEffect(() => {
    (async function () {
      const text  = await (await fetch(`/api/message`)).text();
      setData(text);
      console.debug(data);
    })();
  });

  return (
    <>
      <p>Hello</p>
      <div>{data}</div>
    </>
  );
}

export default App;
