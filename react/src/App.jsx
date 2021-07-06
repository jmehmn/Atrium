import React, { useState } from "react";
import Navbar from "./components/base/NavBar";

import debug from "coti-debug";

import { useRoutes } from "hookrouter";
import Routes from "./routes";

import "bootstrap/dist/css/bootstrap.min.css";

const _logger = debug;

function App() {
  const [user, setUser] = useState();
  const routeResult = useRoutes(Routes);
  _logger(routeResult);
  return (
    <div className="App">
      <Navbar user={user} />
      {routeResult}
    </div>
  );
}

export default App;
