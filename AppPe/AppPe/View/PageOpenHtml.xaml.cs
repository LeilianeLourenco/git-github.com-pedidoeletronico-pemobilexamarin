using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View
{
    public partial class PageOpenHtml : ContentPage
    {
        public PageOpenHtml(string xHtml, string xTile)
        {
            InitializeComponent();
            Title = xTile;
            var htmlSource = new HtmlWebViewSource();


            if (xHtml.ToUpper().Equals("CHAT"))
            {
                #region Chat Online
                xHtml = @"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta name='viewport' content='width=device-width' />
                            <title>Ajuda</title>                            
                        </head>
                      <body style='background: #dddddd !important'>

                            <div style='text-align:center !important ; position:relative !important'>

                                <span style='font-size:23px !important ; font-weight: 600 !important ; color: #555555 !important ; font-family: Segoe UI, Tahoma, Geneva, Verdana, sans-serif !important; margin: 0 auto !important'>
                                    Chat On-line
                                </span>

                                <div>
                                    <span style='font-size:17px !important ; font-weight: 600 !important ; color: #757575 !important ; font-family: Segoe UI, Tahoma, Geneva, Verdana, sans-serif !important; margin: 0 auto !important'>
                                        Clique no botão de ação em azul para abrir o chat com nossos técnicos.
                                    </span>
                                </div>

                            </div>

                        <script src='https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js'></script>
                        <script id='ze-snippet' src='https://static.zdassets.com/ekr/snippet.js?key=a9a732e1-57d9-42ef-b3c6-a21dc7472898'></script>
                        </body>
                        </html>";

                #endregion
            }
            else if (xHtml == "TERMOS")
            {
                #region termos
                xHtml = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <meta name='viewport' content='width=device-width' />
                                <title>Chat on-line</title>
                            </head>
                            <body style='background:#dddddd !important'>    
                               <div class='container' style='font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif !important; font-weight:500 !important; color:#757575 !important ; padding:13px !important'>
                                <div class='text-center'>
                                    <h2>TERMOS DE CONDIÇÃO DE USO DO APLICATIVO</h2>
                                    <br />
                                </div>

                                <p>
                                    Estes “Termos e Condições de Uso do Aplicativo” é um acordo legal entre o licenciado (pessoa física ou jurídica) (o “LICENCIADO”) e pela <strong>HLP Mobile Services ME</strong>,
                                     estabelecida a Rua Nove de Julho, 709 – Vila Nova – Edifício Novo Centro, 6º Andar, Sala 63 - Salto/SP, inscrita no  C.N.P.J. sob o nr. 24.167.731/0001-94 sociedade limitada, 
                                    denominada simplesmente CONTRATADA, de nome fantasia “<strong>HLP Mobile Services</strong>”, doravante designada <u>CONTRATADA</u> e de outro o <u>LICENCIADO</u>.
                                </p>
                                <br />
                                <strong></strong>
                                <br />
                                <div>
                                    <h2>
                                        1.	OBJETO
                                    </h2>
                                    <br />
                                    <p>
                                        <strong>a.</strong>	A CONTRATADA nos termos, cláusulas e condições estipuladas neste instrumento, obriga-se a fornecer ao LICENCIADO, o aplicativo pedidoeletronico.com.
                                    </p>
                                    <p>
                                        <strong>b.</strong>	Aplica-se a este contrato o disposto nas leis 9.609/98 (Proteção da propriedade intelectual do software) e 9.610/98 (Proteção dos direitos autorais).
                                    </p>
                                    <p>
                                        <strong>c.</strong>	O presente contrato em nenhum momento refere-se à venda ou transferência de propriedade, mas tão somente à licença não exclusiva de utilização de cópias do aplicativo. A propriedade intelectual sobre o software e aplicativo não é objeto deste contrato.
                                    </p>
                                    <p>
                                        <strong>d.</strong>	É vedado ao LICENCIADO o sublicenciamento, a consignação, a locação ou a venda do objeto deste.
                                    </p>
                                    <p>
                                        <strong>e.</strong>	O LICENCIADO, não poderá modificar, traduzir, decompor, adaptar ou mesmo aplicar qualquer tipo de engenharia reversa sobre o produto, nem mesmo criar dele derivados, quer seja de suas partes ou de seu todo.
                                    </p>
                                </div>
                                <br />

                                <br />
                                <div>
                                    <h2>
                                        2.	TERMOS E CONDIÇÕES
                                    </h2>
                                    <br />
        
                                    <p>
                                        <strong>a.</strong>	O LICENCIADO SE OBRIGA:
                                    </p>
                                    <ul>
                                        <li>
                                            <p>
                                                <strong>i.</strong>	Ter ciência dos direitos e obrigações decorrentes do presente, constituindo este instrumento o acordo completo entre as partes. Declara, ainda, ter lido, compreendido e aceito todos os termos e condições deste instrumento.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>ii.</strong> Ter ciência de que as operações que corresponderem à aceitação de determinadas opções serão registradas nos bancos de dados a CONTRATADA, juntamente com a data e hora em que o aceite foi manifestado pelo LICENCIADO, podendo tal informação ser utilizada como prova da aceitação da opção pelo LICENCIADO, independentemente de outra formalidade.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>iii.</strong> Colocará a inteira disposição o celular, smartphone, tablete ou computador (equipamentos) e softwares básicos, para que a CONTRATADA, possa executar seus serviços.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>iv.</strong> Manter os equipamentos básicos como Sistemas operacionais atualizados tecnologicamente, atendendo às exigências mínimas de recursos necessários ao processamento correto, com desempenhos satisfatórios aos usuários e principalmente que garantam a segurança das informações.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>v.</strong>	A partir do momento em que o LICENCIADO aceitar este Contrato, as disposições aqui constantes regerão a relação entre a a CONTRATADA e o LICENCIADO, razão pela qual é recomendável que o LICENCIADO imprima uma cópia deste documento para futura referência.
                                            </p>
                                        </li>
                                    </ul>
        
                                    <p>
                                        <strong>b.</strong>	A CONTRATADA SE OBRIGA:
                                    </p>
                                    <ul>
                                        <li>
                                            <p>
                                                <strong>i.</strong>	Fornecer o aplicativo e o portal em código objeto, atendendo ao processamento a que se destinam.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>ii.</strong> Analisar, discutir, alterar e/ou criar fórmulas, tabelas e relatórios para atendimento ao LICENCIADO. Caso não haja tempo hábil para implementar as modificações legais entre a divulgação e o início de vigência das mesmas, a CONTRATADA indicará as soluções alternativas para atender, temporariamente, às exigências da nova lei, até que os módulos possam ser atualizados.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>iii.</strong> Prestar assistência técnica necessária à utilização dos sistemas, via telefone e presencial. Para validade da assistência técnica (suporte), a LICENCIADO deverá contatar a CONTRATADA.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>iv.</strong>Não compreende como “obrigação da CONTRATADA” e devem ser remunerados de acordo com o valor de hora técnica ou negociado a parte formalmente, os seguintes:
                                            </p>
                                        </li>
                                        <li>
                                            <ul>
                                                <li>
                                                    <p>
                                                        <strong>1.</strong>	Correções de erros, ou recuperação de arquivos, banco de dados, provenientes de operação e uso indevido dos softwares, de falhas do equipamento, ou do sistema operacional, ou da instalação elétrica ou de erros em outros aplicativos e sistemas do LICENCIADO.
                                                    </p>
                                                </li>
                                                <li>
                                                    <p>
                                                        <strong>2.</strong>	Desenvolvimento de funcionalidades específicas do aplicativo, de interesse do LICENCIADO.
                                                    </p>
                                                </li>
                                                <li>
                                                    <p>
                                                        <strong>3.</strong>	Treinar funcionários do LICENCIADO para utilização do software de suporte remoto e manuseio do software e aplicativo e seu gerenciamento.
                                                    </p>
                                                </li>
                                                <li>
                                                    <p>
                                                        <strong>4.</strong>	As mensalidades previstas neste contrato, não cobrirá suporte presencial, novas rotinas e novos relatórios; o que neste caso deverá ser pago separadamente, por hora técnica trabalhada e aprovada pelo LICENCIADO; assim como as despesas de transporte, alimentação, estadias, comunicação e outras necessárias a este fim.
                                                    </p>
                                                </li>
                                            </ul>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>v.</strong>	O horário de atendimento da CONTRATADA, na sua sede, vendedores, franquias ou pontos de apoio, é das 07:30 HS as 12:00 HS e de 13:12 HS as 17:30 HS, de segundas à quintas-feiras e de sextas-feiras das 07:30 HS as 12:00 HS e de 13:12 HS as 16:30 HS, em todos os dias úteis, considerados pela legislação na cidade da CONTRATADA. Se o LICENCIADO necessitar de atendimento fora destes horários e períodos, deverá ser solicitado e negociado com antecedência os custos e prazos de atendimentos.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>vi.</strong> Em função de necessidades comerciais ou de legislação local, em algumas unidades da CONTRATADA o horário poderá variar em 30 (trinta) minutos para mais ou menos no horário de início e fim de expediente.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>vii.</strong> Modificações, novos relatórios e melhorias poderão ser sugeridos pelo LICENCIADO, mas a decisão de implementar ou não, ficará a cargo exclusivo da CONTRATADA.
                                            </p>
                                        </li>
                                    </ul>
                                </div>
                                <br />

                                <br />
                                <div>
                                    <h2>
                                        3.	RESPONSABILIDADE
                                    </h2>
                                    <br />
        
                                    <p>
                                        <strong>a.</strong>	A CONTRATADA se compromete a manter a confidencialidade, integridade e em segurança quaisquer Informações disponibilizadas pelo LICENCIADO relacionadas a seus Usuários e/ou Equipamentos de TI.
                                    </p>
                                    <p>
                                        <strong>b.</strong>	Em nenhuma circunstância terá a CONTRATADA, responsabilidade sobre dados indiretos, acidentais, especiais ou conseqüenciais ou por qualquer perdas de lucros, economias, receitas ou dados decorrentes ou relativos ao uso dos softwares disponibilizados ou ainda decorrentes de causas externas como falha no hardware, falta de energia, instalação e utilização indevida, Parametrização indevida, ou configuração dos sistemas operacionais, banco de dados e outros necessários ao funcionamento no smartphones ou tabletes.
                                    </p>
                                    <p>
                                        <strong>c.</strong>	Com a finalidade de garantir a privacidade do LICENCIADO, bem como a segurança de suas Informações, a CONTRATADA se compromete a regularmente reavaliar a sua política de segurança e adaptá-la, conforme necessário.
                                    </p>
                                    <p>
                                        <strong>d.</strong>	Em nenhuma hipótese, a CONTRATADA irá vender ou disponibilizar as Informações do LICENCIADO ou geradas através do uso do Aplicativo, sendo certo que somente as utilizará para as seguintes finalidades, com as quais o LICENCIADO expressamente concorda e aqui:
                                    </p>
                                    <ul>
                                        <li>
                                            <p>
                                                <strong>i.</strong>	Para enviar ao LICENCIADO qualquer notificação administrativa, alertas e comunicados relevantes para o mesmo;
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>ii.</strong> Para cumprir com a finalidade do Portal ou do Aplicativo;
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>iii.</strong> Identificar o perfil, desejos ou necessidades do LICENCIADO a fim de aprimorar os produtos e/ou serviços oferecidos pelo Aplicativo ou Portal;
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>iv.</strong> Realizar estatísticas genéricas para monitoramento de utilização do Aplicativo;
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>v.</strong>	Para pesquisas de marketing, planejamento de projetos da CONTRATADA;
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>vi.</strong> Resolução de problemas no Portal ou Aplicativo, verificação e proteção das Informações, do Portal ou Aplicativo contra erros, fraudes ou qualquer outro crime eletrônico.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>vii.</strong> Caso haja qualquer alteração nas informações de cadastro fornecidas pelo LICENCIADO, este se compromete a informar a CONTRATADA de tais alterações de modo a garantir o correto uso e funcionamento do Aplicativo.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>viii.</strong> As Informações do LICENCIADO poderão ser transferidas a terceiros em decorrência da venda, aquisição, fusão, reorganização societária ou qualquer outra mudança no controle da CONTRATANTE. Caso ocorra qualquer destas hipóteses, no entanto, resultando na transferência das Informações a terceiros, o LICENCIADO será informado previamente e caso não deseje prosseguir com a utilização do Aplicativo, poderá excluir sua conta de acesso, as Informações do LICENCIADO serão apagadas do banco de dados da CONTRATANTE e não serão transmitidas a terceiros.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>ix.</strong> Não obstante as Informações fornecidas pelo LICENCIADO estejam seguras nos termos deste contrato, o login e a senha de acesso ao Aplicativo são confidenciais e de responsabilidades exclusiva do LICENCIADO.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>x.</strong>	Caso o LICENCIADO acredite que seu login e senha de acesso ao Portal tenham sido roubados ou sejam de conhecimento de outras pessoas, por qualquer razão, o LICENCIADO deverá imediatamente comunicar a CONTRATADA.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>xi.</strong> O LICENCIADO compromete-se a não acessar outras áreas ou outros bancos de dados a não ser aqueles necessários para a sua atualização e manutenção de seus próprios serviços.
                                            </p>
                                        </li>
                                    </ul>
                                    <p>
                                        <strong>e.</strong>	Descumprindo tal sigilo, responderá por todos os danos que a divulgação de tais informações gerarem. A eventual divulgação de informações contidas nos softwares por terceiros não é de responsabilidade da CONTRATADA, esta se compromete somente em não divulgar as informações sigilosas do LICENCIADO, que casualmente venha a ter acesso.
                                    </p>
                                </div>
                                <br />

                                <br />
                                <div>
                                    <h2>
                                        4.	PENALIDADES
                                    </h2>
                                    <br />
                                    <p>
                                        <strong>a.</strong>	Quaisquer modificações feitas nos sistemas ou base de dados, por pessoas não autorizadas pela CONTRATADA, acionará imediatamente a cláusula ‘C’ do item 4.
                                    </p>
                                    <p>
                                        <strong>b.</strong>	Todos os aplicativos são de uso exclusivo do LICENCIADO, para processamento de dados/informações do LICENCIADO, vetando, portanto, a execução de serviços de outras empresas ou de terceiros, também acionando imediatamente a cláusula ‘C’ do item 4.
                                    </p>
                                    <p>
                                        <strong>c.</strong>	Infrações às cláusulas 4.a e 4.b que resulte na divulgação, reprodução total ou parcial dos sistemas e manuais ou situações oriundas que também resulte em prejuízo à CONTRATADA, sujeita ao LICENCIADO, independente de interpelação judicial, a multa equivalente a 50 (cinqüenta) salários mínimos, convertidos em moeda corrente do país na data do pagamento, por cópia ou manuais dos sistemas aplicativos, direta ou indiretamente divulgados, por cada processo indevido, prejuízo da imediata cessão da prática da infração e das ações cabíveis.
                                    </p>
                                </div>
                                <br />

                                <br />
                                <div>
                                    <h2>
                                        5.	CUSTOS E INVESTIMENTOS
                                    </h2>
                                    <br />
                                    <p>            
                                        <strong>a.</strong> O LICENCIADO deve pagar à CONTRATADA o valor do respectivo ao plano escolhido e seus plug-ins e serviços adicionais de acordo com a periodicidade definida entre as opções de pagamento disponibilizadas ao LICENCIADO no ato da contratação, caso o LICENCIADO opte pelo plano gratuito o acesso ao portal e todas suas funcionalidades serão bloqueadas ao LICENCIADO.
                                    </p>
                                    <p>
                                        <strong>b.</strong>	Caso o LICENCIADO, no decorrer da vigência do presente instrumento, opte por outro plano de licenciamento, os valores serão alterados de forma proporcional (pro rata) de acordo com o respectivo plano escolhido.
                                    </p>
                                    <p>
                                        <strong>c.</strong>	A falta de pagamento nas datas determinadas para seu vencimento acarretará na suspensão de acesso ao SOFTWARE até que as pendências financeiras sejam regularizadas, sendo liberado o acesso somente ao aplicativo do pedidoeletronico.com não dando direito do uso do portal.
                                    </p>
                                    <p>
                                        <strong>d.</strong>	Caso a suspensão permaneça por prazo superior a 60 (sessenta) dias, a CONTRATADA poderá excluir integralmente as informações lançadas no portal pelo LICENCIADO.
                                    </p>
                                    <p>
                                        <strong>e.</strong>	Os valores estabelecidos no ato da formalização do aceite são para pagamento antecipado pelo LICENCIADO para posterior uso. Este valor poderá ser alterado a qualquer tempo pela CONTRATADA, podendo o LICENCIADO não renovar o acesso ao portal e aplicativo caso discorde dos novos valores apresentados pela CONTRATADA. Neste caso, o acesso ao portal será suspenso e o aplicativo terá acesso a suas limitações do plano gratuito findo o período de validade do pagamento anterior.
                                    </p>
                                </div>
                                <br />

                                <br />
                                <div>
                                    <h2>
                                        6.	 LIMITAÇÃO DE RESPONSABILIDADE E INDENIZAÇÃO
                                    </h2>
                                    <br />

                                    <p>
                                        <strong>a.</strong>	A CONTRATADA não responderá, em nenhuma hipótese, ainda que em caráter solidário ou subsidiário:
                                    </p>
                                    <ul>
                                        <li>
                                            <p>
                                                <strong>i.</strong>	Por eventuais prejuízos sofridos pelo LICENCIADO em razão da tomada de decisões com base nas informações disponibilizadas no Portal ou Aplicativo;
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>ii.</strong> Por eventuais prejuízos sofridos pelo LICENCIADO em razão de falhas no sistema de informática ou nos servidores que independam de culpa da CONTRATADA ou em sua conectividade com a internet de modo geral, devendo o LICENCIADO manter, às suas expensas, linha de telecomunicação, modem, software de comunicação, endereço de correio eletrônico e outros recursos necessários à comunicação com a CONTRATADA.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                <strong>iii.</strong> Por situações de caso fortuito ou força maior, nos termos do artigo 393 do Código Civil Brasileiro.
                                            </p>
                                        </li>
                                    </ul>
                                    <p>
                                        <strong>b.</strong>	A CONTRATADA não garante que as funções contidas no Aplicativo atendam às suas necessidades, que a operação do Aplicativo será ininterrupta ou livre de erros, que qualquer funcionalidade continuará disponível, que os defeitos no Aplicativo serão corrigidos ou que o Aplicativo será compatível ou funcione com qualquer Aplicativo, aplicações ou serviços de terceiros.
                                    </p>
                                    <p>
                                        <strong>c.</strong>	Em nenhum caso a CONTRATADA será responsável por danos pessoais ou qualquer prejuízo incidental, especial, indireto ou consequente, lucros cessantes, incluindo, sem limitação, prejuízos por perda de lucro, corrupção ou perda de dados, falha de transmissão ou recepção de dados, não continuidade do negócio ou qualquer outro prejuízo ou perda comercial, decorrentes ou relacionados ao seu uso ou sua inabilidade em usar o Aplicativo, por qualquer outro motivo.
                                    </p>
                                    <p>
                                        <strong>d.</strong>	Na eventualidade da CONTRATADA ser compelida, por decisão judicial transitada em julgado, a indenizar ou ressarcir o LICENCIADO por danos sofridos, apesar do disposto no Item i do item 7 acima, o valor devido ao LICENCIADO será limitado à 20% (vinte por cento) da totalidade da quantia efetivamente paga pelo LICENCIADO à CONTRATADA a título de fruição das funcionalidades oferecidas pelo Aplicativo.
                                    </p>
                                </div>
                                <br />

                                <br />
                                <div>
                                    <h2>
                                        7.	LEI E FORO APLICÁVEIS
                                    </h2>
                                    <br />

                                    <p>
                                        <strong>a.</strong>	Este Termo será interpretado exclusivamente segundo as leis do Brasil.
                                    </p>
                                    <p>
                                        <strong>b.</strong>	As partes elegem o Foro da Comarca de Salto, Estado de São Paulo, como o único competente para dirimir qualquer litígio resultante deste Termo.
                                    </p>
                                    <p>
                                        <strong>c.</strong>	Este Termo constitui a totalidade do acordo sobre as condições de uso do Portal e do Aplicativo. O LICENCIADO declara ter ciência dos direitos e obrigações decorrentes do presente termo, tendo lido, compreendido e aceito todos os termos e condições.
                                    </p>
                                </div>
                                <br />   

                                <br />
                                <div class='text-center'>
                                    <p>
                                        <strong>Salto, 16 de março de 2016.</strong><br />
                                    </p>       

                                </div>    
                                <br />

                                <br />
                                <div>
        
                                </div>
                                <br />

                            </div>
                            </body>
                            </html>";
                #endregion 
            }
            htmlSource.Html = xHtml;
            Browser.Source = htmlSource;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
