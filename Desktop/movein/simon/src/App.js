import logo from './logo.svg';
import './App.css';
import Aboutme from './components/Aboutme/Aboutme';
import React, { Component } from 'react'
import Town from './components/Town/Town'


class App extends Component {
  constructor(props) {
    super(props)
    this.state = {
      selectedMenu: 'p'
    }
  }

  render() {
    return (
      <div className="App">

        <div className="menu">
          <p className="menu-item" onClick={() => this.setState({ selectedMenu: 'p' })}>About Me</p>
          <p className="menu-item" onClick={() => this.setState({ selectedMenu: 't' })}>My Town</p>
        </div>

        {this.state.selectedMenu === 'p' ?
          <Aboutme />
          :
          <Town />
        }
      </div>
    );
  }
}


export default App;