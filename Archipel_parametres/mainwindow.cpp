#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    QXmlStreamReader Rxml;

        QString filename = "parametres.xml";

        QFile file(filename);
        if (file.open(QFile::ReadOnly | QFile::Text))
        {
            qDebug()<<"toto";
            Rxml.setDevice(&file);
            Rxml.readNext();

            while(!Rxml.atEnd()){
                if(Rxml.isStartElement()){
                    if(Rxml.name() == "networkListeningPort")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                    ui->networkListeningPort->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "networkSendingPort")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                    ui->networkSendingPort->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "networkServerIP")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->networkServerIP->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "pirateBoatsInitInterval")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->pirateBoatsInitInterval->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "pirateBoatsIncreaseRate")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->pirateBoatsIncreaseRate->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else{
                        Rxml.readNext();
                    }
                }
                else{
                    Rxml.readNext();
                }
            }

            file.close();

            if (Rxml.hasError()){
               std::cerr << "Error: Failed to parse file "
                         << qPrintable(filename) << ": "
                         << qPrintable(Rxml.errorString()) << std::endl;
            }
            else if (file.error() != QFile::NoError){
                std::cerr << "Error: Cannot read file " << qPrintable(filename)
                          << ": " << qPrintable(file.errorString())
                          << std::endl;
            }
        }
}

void MainWindow::closeEvent(QCloseEvent *event)
{

    QString filename = "parametres.xml";
    QFile file(filename);
    file.open(QIODevice::WriteOnly);

    QXmlStreamWriter xmlWriter(&file);
    xmlWriter.setAutoFormatting(true);
    xmlWriter.writeStartDocument();

    xmlWriter.writeStartElement("Archipel");

        xmlWriter.writeStartElement("networkListeningPort");
        xmlWriter.writeTextElement("Value", ui->networkListeningPort->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("networkSendingPort");
        xmlWriter.writeTextElement("Value", ui->networkSendingPort->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("networkServerIP");
        xmlWriter.writeTextElement("Value", ui->networkServerIP->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("pirateBoatsInitInterval");
        xmlWriter.writeTextElement("Value", ui->pirateBoatsInitInterval->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("pirateBoatsIncreaseRate");
        xmlWriter.writeTextElement("Value", ui->pirateBoatsIncreaseRate->text());
        xmlWriter.writeEndElement();

    xmlWriter.writeEndElement();

    file.close();

    delete ui;
}

void MainWindow::clearScores() {
    qDebug()<<"lel";

    QMessageBox messageBox(QMessageBox::Question, tr("Erase scores"),
                               tr("Are you sure you want to erase all the stored scores ?"),
                               QMessageBox::Yes | QMessageBox::No);
        int ret = messageBox.exec();

    QString filename = "scores.txt";
    QFile file(filename);
    file.open(QFile::WriteOnly|QFile::Truncate);

    file.close();
}
