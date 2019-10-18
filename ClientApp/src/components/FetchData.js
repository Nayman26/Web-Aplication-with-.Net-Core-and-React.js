import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };

        fetch('api/Sample/Weather')
            .then(response => response.json())
            .then(data => {
                this.setState({ forecasts: data, loading: false });
                //console.log(data);
            });
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>temperatureC</th>
                        <th>temperatureF</th>
                        <th>summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr>
                            <td>{forecast.dateFormatted}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts);

        return (
            <div>
                <h1>Sample Weather Forecast</h1>
                {contents}
            </div>
        );
    }
}
