import React, { Component } from 'react';

export class Deneme extends Component {
    //static displayName = FetchData.name;
    constructor(props) {
        super(props);
        this.fetchData = this.fetchData.bind(this);
        this.state = {
            veriler: [],
            baslik: [],
            secili: "",
            active: null,
            active1: null,
            active2: null,
            isClicked: false,
            isVisible: false,
            visibility: 'hidden',
            header: ''
        };
        fetch('api/Veriler/tum')
            .then(response => response.json())
            .then(data => {
                this.setState({ veriler: data, loading: false });
            });
        fetch('api/Veriler/b4')
            .then(response => response.json())
            .then(data => {
                this.setState({ baslik: data, loading: false });
            });
    }
    createBody = () => {
        let body = []
        for (let i = 0; i < this.state.baslik.length; i++) {
            body.push(<td>
                {

                    this.state.veriler.map((sutun, index) =>
                        <tr
                            style={{ background: this.state.isClicked ? this.fonk3(index, i, 'yellow') : this.fonk1(i, 'yellow') }}
                            onClick={() => this.fonk4(index, i)} /*alert(index + ' ' + i + ' ' + sutun[i])*/
                        >
                            <td >{sutun[i]}</td>
                        </tr>
                    )}
            </td>)
        }
        return body
    }
    createHeader = () => {
        return (
            <tr>
                {this.state.baslik.map((_header, index) =>
                    <th
                        style={{ background: this.fonk1(index, 'red') }}
                        onClick={() => {
                            this.fonk2(index)
                            this.setState({ secili: this.state.baslik[index] })
                            this.state.isVisible ? this.setUnVisible() : this.setVisible()
                        }}
                    >
                        {this.state.baslik[index]}
                    </th>
                )}
            </tr>
        )
    }
    fetchData(body, head) {
        fetch('api/Veriler/' + body)
            .then(response => response.json())
            .then(data => {
                this.setState({ veriler: data });
            });
        fetch('api/Veriler/' + head)
            .then(response => response.json())
            .then(data => {
                this.setState({ baslik: data });
            });
    }
    fonk1(position, color) {
        if (this.state.active === position) {
            return color;
        }
        return "";
    }
    fonk2(position) {
        if (this.state.active === position) {
            this.setState({ active: null, isClicked: true })
        } else {
            this.setState({ active: position, isClicked: false })
        }
    }
    fonk3(row, column, color) {
        if (this.state.active1 === row && this.state.active2 === column) {
            return color;
        }
        return "";
    }
    fonk4(row, column) {
        if (this.state.active1 === row && this.state.active2 === column) {
            this.setState({ active1: null, active2: null, isClicked: true })
        } else {
            this.setState({ active1: row, active2: column, isclicked: false })
        }
    }
    setVisible() {
        this.setState({
            visibility: 'visible',
            isVisible: true
        });
    }
    setUnVisible() {
        this.setState({
            visibility: 'hidden',
            isVisible: false
        });
    }
    render() {
        return (
            <div>
                <div>
                    <button onClick={() => {
                        this.fetchData('kisisel', 'b1'),
                        this.setState({ header: 'Kisisel Veriler' })
                    }}>Kisisel</button>
                    <button onClick={() => {
                        this.fetchData('cok_kisisel', 'b2'),
                        this.setState({ header: 'Cok Kisisel Veriler' })
                    }}>Cok Kisisel</button>
                    <button onClick={() => {
                        this.fetchData('normal', 'b3'),
                        this.setState({ header: 'Normal Veriler' })
                    }}>Normal</button>
                    <button onClick={() => {
                        this.fetchData('tum', 'b4'),
                        this.setState({ header: 'Tüm Veriler' })
                    }}>Tüm</button>
                </div>
                <div style={{ marginTop: '10px', marginBottom: '10px', visibility: this.state.visibility }}>
                    <button onClick={() => alert(this.state.secili + " sütünü kisisel yapıldı")}>Kisisel Yap</button>
                    <button onClick={() => alert(this.state.secili + " sütünü cok kisisel yapıldı")}>Cok Kisisel Yap</button>
                    <button onClick={() => alert(this.state.secili + " sütünü normal yapıldı")}>Normal Yap</button>
                </div>
                <div>
                    <h3>
                        {this.state.header}
                    </h3>
                    <table className='table table-striped'>
                        <thead>
                            {this.createHeader()}
                        </thead>
                        <tbody>
                            {this.createBody()}
                        </tbody>
                    </table>
                </div>
            </div>
        )
    }
}